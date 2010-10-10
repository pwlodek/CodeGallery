using System;
using System.AddIn.Pipeline;
using Plugins.Sdk.Contracts;
using Plugins.Sdk.Views;
using System.Windows;
using System.AddIn.Contract;

namespace Plugins.Sdk.AddInAdapters
{
    [AddInAdapter]
    public class WpfAddInViewAdapter : ContractBase, IWpfAddIn
    {
        private readonly WpfAddinView m_View;

        public WpfAddInViewAdapter(WpfAddinView view)
        {
            m_View = view;
        }

        #region IWpfAddIn Members

        public INativeHandleContract RegisterContent()
        {
            FrameworkElement element = m_View.RegisterContent();
            INativeHandleContract contract = FrameworkElementAdapters.ViewToContractAdapter(element);

            return contract;
        }

        #endregion
    }
}
