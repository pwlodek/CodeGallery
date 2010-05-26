using System;
using System.ComponentModel.Composition;
using Extensible.Dashboard.Views.Presenters;

namespace Extensible.Dashboard.Views.Windows
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    [Export(typeof(IShellView))]
    public partial class ShellWindow : IShellView
    {
        public ShellWindow()
        {
            InitializeComponent();
        }

        public ShellPresentationModel PresentationModel
        {
            get { return (ShellPresentationModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
