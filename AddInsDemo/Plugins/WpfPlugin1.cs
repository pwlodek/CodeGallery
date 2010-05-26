using System;
using Plugins.Controls;
using Plugins.Sdk.Views;
using System.Windows;
using System.AddIn;

namespace Plugins
{
    [AddIn("My First WPF Add-In", Version = "1.0.0.0", Description = "Description of My First Add-In", Publisher = "AGH Univ.")]
    public class WpfPlugin1 : WpfAddinView
    {
        public override FrameworkElement RegisterContent()
        {
            return new UserControl1();
        }
    }
}
