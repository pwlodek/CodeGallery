using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Contoso.Extensible.Dashboard.Widgets.ViewModels;

namespace Contoso.Extensible.Dashboard.Widgets.Views.Presenters
{
    [Export]
    public class CurrencyWidgetPresentationModel
    {
        public ObservableCollection<Currency> CurrencyCollection { get; private set; }

        public CurrencyWidgetPresentationModel()
        {
            CurrencyCollection = new ObservableCollection<Currency>();

            CurrencyCollection.Add(new Currency { Symbol = "EUR", BuyPrice = 4.00m, SellPrice = 4.12m });
            CurrencyCollection.Add(new Currency { Symbol = "USD", BuyPrice = 3.21m, SellPrice = 3.45m });
            CurrencyCollection.Add(new Currency { Symbol = "CHF", BuyPrice = 2.85m, SellPrice = 2.91m });
            CurrencyCollection.Add(new Currency { Symbol = "GBP", BuyPrice = 4.66m, SellPrice = 4.73m });
        }
    }
}