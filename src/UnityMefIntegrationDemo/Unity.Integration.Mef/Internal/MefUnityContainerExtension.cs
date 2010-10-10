/// --------------------------------------------------------------------------------------
/// <copyright file="MefUnityContainerExtension.cs">
///     Copyright (C) Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains internal MefUnityContainerExtension class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Unity.Integration.Mef.Internal
{
    /// <summary>
    /// Represents a Unity extension that adds integration with
    /// Managed Extensibility Framework.
    /// </summary>
    internal class MefUnityContainerExtension : UnityContainerExtension
    {
        private readonly ComposablePartCatalog m_Catalog;
        private readonly bool m_Register;
        private readonly ExportProvider[] m_Providers;

        public MefUnityContainerExtension(ComposablePartCatalog catalog, params ExportProvider[] providers)
            : this(catalog, true, providers)
        {
        }

        public MefUnityContainerExtension(ComposablePartCatalog catalog, bool register, params ExportProvider[] providers)
        {
            Debug.Assert(catalog != null);
            m_Catalog = catalog;
            m_Register = register;
            m_Providers = providers;
        }

        protected override void Initialize()
        {
            CompositionContainer compositionContainer = PrepareCompositionContainer();

            Context.Strategies.AddNew<MefLifetimeStrategy>(UnityBuildStage.Lifetime);
            Context.Strategies.AddNew<MefBuilderStrategy>(UnityBuildStage.Initialization);
            Context.Locator.Add(typeof(CompositionContainer), compositionContainer);
        }

        private CompositionContainer PrepareCompositionContainer()
        {
            // Create the MEF container based on the catalog
            var compositionContainer = new CompositionContainer(m_Catalog, m_Providers);

            // Create composition batch and add the MEF container and the Unity
            // container to the MEF
            var batch = new CompositionBatch();
            batch.AddExportedObject(compositionContainer);
            batch.AddExportedObject(Container);

            // Prepare container
            compositionContainer.Compose(batch);

            if (m_Register)
            {
                Container.RegisterInstance(compositionContainer);
            }

            return compositionContainer;
        }

        public bool Register
        {
            get { return m_Register; }
        }

        public ComposablePartCatalog Catalog
        {
            get { return m_Catalog; }
        }

        #region Builder Strategies

        /// <summary>
        /// Represents a strategy which injects MEF dependencies to
        /// the Unity created object.
        /// </summary>
        private class MefBuilderStrategy : BuilderStrategy
        {
            public override void PostBuildUp(IBuilderContext context)
            {
                Type type = context.Existing.GetType();
                object[] attributes = type.GetCustomAttributes(typeof (PartNotComposableAttribute), false);

                if (attributes.Length == 0)
                {
                    var container = context.Locator.Get<CompositionContainer>();
                    container.SatisfyImports(context.Existing);
                }
            }
        }

        /// <summary>
        /// Represents a MEF lifetime strategy which tries to resolve desired
        /// component via MEF. If succeeded, build process is completed.
        /// </summary>
        private class MefLifetimeStrategy : BuilderStrategy
        {
            public override void PreBuildUp(IBuilderContext context)
            {
                var container = context.Locator.Get<CompositionContainer>();
                var buildKey = (NamedTypeBuildKey) context.BuildKey;

                try
                {
                    var exports = container.GetExports(buildKey.Type, null, buildKey.Name);
                    
                    if (exports.Count == 0)
                        return;

                    if (exports.Count > 1)
                        throw new ArgumentException("Requested type has more than one instance.");

                    if (exports[0].Metadata.ContainsKey(Constants.IsUnityProvided))
                        return;

                    context.Existing = exports[0].GetExportedObject();
                    context.BuildComplete = true;
                }
                catch (Exception)
                {
                    context.BuildComplete = false;
                    throw;
                }
            }
        }

        #endregion
    }
}