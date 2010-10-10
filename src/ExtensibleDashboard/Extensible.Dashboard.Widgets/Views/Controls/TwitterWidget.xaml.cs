using System;
using System.ComponentModel.Composition;
using Extensible.Dashboard.Contracts;
using Extensible.Dashboard.Widgets.Views.Presenters;

namespace Extensible.Dashboard.Widgets.Views.Controls
{
    /// <summary>
    /// Interaction logic for TwitterWidget.xaml
    /// </summary>
    [Export(typeof(IWidget))]
    [ExportMetadata("Location", WidgetLocation.Left)]
    public partial class TwitterWidget : ITwitterWidget
    {
        public TwitterWidget()
        {
            InitializeComponent();
        }

        [Import]
        public TwitterWidgetPresentationModel PresentationModel
        {
            get { return (TwitterWidgetPresentationModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
