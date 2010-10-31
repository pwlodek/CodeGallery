using System;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using MefContrib.Hosting.Interception;
using MefContrib.Hosting.Interception.Configuration;

namespace MefContribDemo.Interception
{
    public class LogInterceptionCriteria : IPartInterceptionCriteria
    {
        public LogInterceptionCriteria(IExportedValueInterceptor interceptor)
        {
            Interceptor = interceptor;
        }

        public IExportedValueInterceptor Interceptor { get; private set; }

        public Func<ComposablePartDefinition, bool> Predicate
        {
            get
            {
                return def => def.ExportDefinitions.First().Metadata.ContainsKey("Log") &&
                              def.ExportDefinitions.First().Metadata["Log"].Equals(true);
            }
        }
    }
}