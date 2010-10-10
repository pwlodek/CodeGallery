using System;
using System.ComponentModel.Composition;

namespace CompositeWpfDemo.Module1.Views.Controls
{
    /// <summary>
    /// Interaction logic for DemoView.xaml
    /// </summary>
    [Export(typeof(IDemoView))]
    public partial class DemoView : IDemoView
    {
        public DemoView()
        {
            InitializeComponent();
        }
    }
}
