/// --------------------------------------------------------------------------------------
/// <copyright file="MefUnityContainerExtension.cs">
///     Copyright (C) 2008-2009 Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains internal UnityExportDefinition class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace Unity.Integration.Mef.Internal
{
    internal class UnityExportDefinition : ExportDefinition
    {
        private readonly string m_ContractName;
        private readonly IDictionary<string, object> m_Metadata;

        public UnityExportDefinition(Type type)
            : this(type, null)
        {
        }

        public UnityExportDefinition(Type type, string unityRegistrationName)
        {
            m_Metadata = new Dictionary<string, object>();
            m_ContractName = unityRegistrationName ?? type.FullName;

            ServiceType = type;
            UnityRegistrationName = unityRegistrationName;

            m_Metadata.Add(Constants.ExportTypeIdentity, type.FullName);
            m_Metadata.Add(Constants.IsUnityProvided, true);
        }

        public override string ContractName
        {
            get { return m_ContractName; }
        }

        public override IDictionary<string, object> Metadata
        {
            get { return m_Metadata; }
        }

        public Type ServiceType { get; private set; }
        public string UnityRegistrationName { get; private set; }
    }
}