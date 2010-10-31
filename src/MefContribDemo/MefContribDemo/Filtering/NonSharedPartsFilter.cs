using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using MefContrib.Hosting.Interception;

namespace MefContribDemo.Filtering
{
    public class NonSharedPartsFilter : IExportHandler
    {
        private const string MetadataName = CompositionConstants.PartCreationPolicyMetadataName;
        
        public void Initialize(ComposablePartCatalog interceptedCatalog)
        {
        }

        public IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(
            ImportDefinition definition,
            IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> exports)
        {
            return from export in exports
                   where export.Item1.Metadata.ContainsKey(MetadataName) &&
                         export.Item1.Metadata[MetadataName].Equals(CreationPolicy.NonShared)
                   select export;
        }
    }
}