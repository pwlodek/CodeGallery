using System;
using System.ComponentModel.Composition;
using Contoso.Extensible.Dashboard.Widgets.Views.Presenters;
using Extensible.Dashboard.Contracts;

namespace Contoso.Extensible.Dashboard.Widgets.Views.Controls
{
    /// <summary>
    /// Interaction logic for CurrencyWidget.xaml
    /// </summary>
    [Widget(WidgetLocation.Right)]
    public partial class CurrencyWidget : IWidget
    {
        public CurrencyWidget()
        {
            InitializeComponent();
        }
        
        [Import]
        public CurrencyWidgetPresentationModel PresentationModel
        {
            get { return (CurrencyWidgetPresentationModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
