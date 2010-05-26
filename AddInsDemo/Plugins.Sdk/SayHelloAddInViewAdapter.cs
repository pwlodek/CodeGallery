using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Pipeline;

namespace Plugins.Sdk
{

    [AddInAdapter]
    public class SayHelloAddInViewAdapter : ContractBase, ISayHelloAddIn
    {
        private SayHelloAddinView _view;

        public SayHelloAddInViewAdapter(SayHelloAddinView view)
        {
            this._view = view;
        }

        public string SayHello(string name)
        {
            return this._view.SayHello(name);
        }
    }
}
