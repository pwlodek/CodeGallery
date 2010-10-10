/// --------------------------------------------------------------------------------------
/// <copyright file="MefUnityContainerExtension.cs">
///     Copyright (C) 2008-2009 Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains internal UnityExportProvider class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using Microsoft.Practices.Unity;

namespace Unity.Integration.Mef.Internal
{
    internal class UnityExportProvider : ExportProvider
    {
        private readonly IUnityContainer m_UnityContainer;
        private readonly IList<UnityExportDefinition> m_Definitions = new List<UnityExportDefinition>();

        public UnityExportProvider(IUnityContainer unityContainer)
        {
            m_UnityContainer = unityContainer;
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition importDefinition)
        {
            return GetExportsCore(Definitions, importDefinition.Constraint.Compile());
        }

        private IEnumerable<Export> GetExportsCore(IEnumerable<UnityExportDefinition> exportDefinitions,
            Func<ExportDefinition, bool> constraint)
        {
            Debug.Assert(exportDefinitions != null);
            Debug.Assert(constraint != null);

            var exports = new List<Export>();
            foreach (var exportDefinition in exportDefinitions)
            {
                if (constraint(exportDefinition))
                {
                    exports.Add(CreateExport(exportDefinition));
                }
            }
            return exports;
        }

        private Export CreateExport(UnityExportDefinition export)
        {
            return new Export(export, () => GetExportedObject(export.ServiceType, export.UnityRegistrationName));
        }

        private object GetExportedObject(Type type, string contractName)
        {
            return m_UnityContainer.Resolve(type, contractName);
        }

        public IList<UnityExportDefinition> Definitions
        {
            get { return m_Definitions; }
        }

        public IUnityContainer UnityContainer
        {
            get { return m_UnityContainer; }
        }
    }
}