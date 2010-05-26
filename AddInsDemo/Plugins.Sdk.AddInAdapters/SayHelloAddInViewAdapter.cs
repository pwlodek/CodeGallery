using System;
using System.AddIn.Pipeline;
using Plugins.Sdk.Contracts;
using Plugins.Sdk.Views;

namespace Plugins.Sdk.AddInAdapters
{

    [AddInAdapter]
    public class SayHelloAddInViewAdapter : ContractBase, ISayHelloAddIn
    {
        private readonly SayHelloAddinView m_View;

        public SayHelloAddInViewAdapter(SayHelloAddinView view)
        {
            m_View = view;
        }

        public string SayHello(string name)
        {
            return m_View.SayHello(name);
        }
    }
}
