using System;
using System.AddIn.Pipeline;
using System.Windows;

namespace Plugins.Sdk.Views
{
    [AddInBase]
    public abstract class WpfAddinView
    {
        public abstract FrameworkElement RegisterContent();
    }
}
