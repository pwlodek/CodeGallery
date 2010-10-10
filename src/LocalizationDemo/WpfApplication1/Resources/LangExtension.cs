using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfApplication1.Resources
{
    [MarkupExtensionReturnType(typeof(object))]
    public class LangExtension : Binding
    {
        public LangExtension(string path)
        {
            Source = PublicResources.Instance;
            Path = new PropertyPath("Resource[" + path + "]");
        }
    }
}
