using System;
using Plugins.Sdk.Views;
using Plugins.Sdk.Contracts;
using System.AddIn.Pipeline;
using System.Windows;
using System.AddIn.Contract;

namespace Plugins.Sdk.HostAdapters
{
    [HostAdapter]
    public class WpfAddInHostAdapter : WpfAddinHostView
    {
        private readonly IWpfAddIn m_Contract;
        private ContractHandle m_Handle;

        public WpfAddInHostAdapter(IWpfAddIn contract)
        {
            m_Contract = contract;
            m_Handle = new ContractHandle(contract);
        }


        public override FrameworkElement RegisterContent()
        {
            INativeHandleContract contract = m_Contract.RegisterContent();
            return FrameworkElementAdapters.ContractToViewAdapter(contract);
        }
    }
}
