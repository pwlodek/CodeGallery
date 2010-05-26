using System;
using System.ComponentModel.Composition;

namespace Extensible.Dashboard.Contracts
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WidgetAttribute : ExportAttribute
    {
        public WidgetAttribute(WidgetLocation location) : base(typeof(IWidget))
        {
            Location = location;
        }

        public WidgetLocation Location { get; private set; }
    }
}