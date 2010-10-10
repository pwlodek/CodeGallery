/// --------------------------------------------------------------------------------------
/// <copyright file="MefUnityContainerExtension.cs">
///     Copyright (C) 2008-2009 Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains public UnityContainerExtensions class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Microsoft.Practices.Unity;
using Unity.Integration.Mef.Internal;

namespace Unity.Integration.Mef
{
    /// <summary>
    /// Class containing extension methods for the Unity IoC.
    /// </summary>
    public static class UnityContainerExtensions
    {
        private static readonly IDictionary<IUnityContainer, MefExtensionHolder> ExtensionHolders =
            new Dictionary<IUnityContainer, MefExtensionHolder>();

        /// <summary>
        /// Registers a MEF catalog within Unity container.
        /// </summary>
        /// <param name="unityContainer">Unity container instance.</param>
        /// <param name="catalog">MEF catalog to be registered.</param>
        public static void RegisterCatalog(this IUnityContainer unityContainer, ComposablePartCatalog catalog)
        {
            EnsureUnityHasHolder(unityContainer);

            ExtensionHolders[unityContainer].Catalog.Catalogs.Add(catalog);
        }

        /// <summary>
        /// Register a type.
        /// </summary>
        /// <typeparam name="TFrom">Type to be registered.</typeparam>
        /// <typeparam name="TTo">Registered type's concrete implementation type.</typeparam>
        /// <param name="unityContainer">The Unity container.</param>
        /// <param name="register">If true, the registration information will be propagated to MEF.</param>
        /// <returns>The Unity container.</returns>
        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer unityContainer, bool register)
            where TTo : TFrom
        {
            if (register)
            {
                EnsureUnityHasHolder(unityContainer);
                ExtensionHolders[unityContainer].ExportProvider.Definitions.Add(
                    new UnityExportDefinition(typeof(TFrom)));    
            }
            
            return unityContainer.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        /// Register a type.
        /// </summary>
        /// <typeparam name="TFrom">Type to be registered.</typeparam>
        /// <typeparam name="TTo">Registered type's concrete implementation type.</typeparam>
        /// <param name="unityContainer">The Unity container.</param>
        /// <param name="name">Name associated with registered type.</param>
        /// <param name="register">If true, the registration information will be propagated to MEF.</param>
        /// <returns>The Unity container.</returns>
        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer unityContainer, string name, bool register)
            where TTo : TFrom
        {
            if (register)
            {
                EnsureUnityHasHolder(unityContainer);
                ExtensionHolders[unityContainer].ExportProvider.Definitions.Add(
                    new UnityExportDefinition(typeof (TFrom), name));
            }

            return unityContainer.RegisterType<TFrom, TTo>(name);
        }

        /// <summary>
        /// Register a type.
        /// </summary>
        /// <typeparam name="TFrom">Type to be registered.</typeparam>
        /// <typeparam name="TTo">Registered type's concrete implementation type.</typeparam>
        /// <param name="unityContainer">The Unity container.</param>
        /// <param name="lifetimeManager">Lifetime manager associated with registered type.</param>
        /// <param name="register">If true, the registration information will be propagated to MEF.</param>
        /// <returns>The Unity container.</returns>
        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer unityContainer, LifetimeManager lifetimeManager, bool register)
            where TTo : TFrom
        {
            if (register)
            {
                EnsureUnityHasHolder(unityContainer);
                ExtensionHolders[unityContainer].ExportProvider.Definitions.Add(
                    new UnityExportDefinition(typeof (TFrom)));
            }

            return unityContainer.RegisterType<TFrom, TTo>(lifetimeManager);
        }

        /// <summary>
        /// Register a type.
        /// </summary>
        /// <typeparam name="TFrom">Type to be registered.</typeparam>
        /// <typeparam name="TTo">Registered type's concrete implementation type.</typeparam>
        /// <param name="unityContainer">The Unity container.</param>
        /// <param name="name">Name associated with registered type.</param>
        /// <param name="lifetimeManager">Lifetime manager associated with registered type.</param>
        /// <param name="register">If true, the registration information will be propagated to MEF.</param>
        /// <returns>The Unity container.</returns>
        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer unityContainer, string name, LifetimeManager lifetimeManager, bool register)
            where TTo : TFrom
        {
            if (register)
            {
                EnsureUnityHasHolder(unityContainer);
                ExtensionHolders[unityContainer].ExportProvider.Definitions.Add(
                    new UnityExportDefinition(typeof (TFrom), name));
            }

            return unityContainer.RegisterType<TFrom, TTo>(name, lifetimeManager);
        }

        /// <summary>
        /// Register existing instance.
        /// </summary>
        /// <typeparam name="TInstance">Instance's type.</typeparam>
        /// <param name="unityContainer">The Unity container.</param>
        /// <param name="instance">Instance to register.</param>
        /// <param name="register">If true, the registration information will be propagated to MEF.</param>
        /// <returns>The Unity container.</returns>
        public static IUnityContainer RegisterInstance<TInstance>(this IUnityContainer unityContainer, TInstance instance, bool register)
        {
            if (register)
            {
                EnsureUnityHasHolder(unityContainer);
                ExtensionHolders[unityContainer].ExportProvider.Definitions.Add(
                    new UnityExportDefinition(typeof(TInstance)));
            }

            return unityContainer.RegisterInstance(instance);
        }

        private static void EnsureUnityHasHolder(IUnityContainer unityContainer)
        {
            if (ExtensionHolders.ContainsKey(unityContainer) == false)
            {
                var extensionHolder = new MefExtensionHolder(unityContainer);
                ExtensionHolders.Add(unityContainer, extensionHolder);

                unityContainer.AddExtension(extensionHolder.MefExtension);
            }
        }

        private class MefExtensionHolder
        {
            public UnityExportProvider ExportProvider { get; private set; }
            public AggregateCatalog Catalog { get; private set; }
            public MefUnityContainerExtension MefExtension { get; private set; }

            public MefExtensionHolder(IUnityContainer unityContainer)
            {
                ExportProvider = new UnityExportProvider(unityContainer);
                Catalog = new AggregateCatalog();
                MefExtension = new MefUnityContainerExtension(Catalog, ExportProvider);
            }
        }
    }
}