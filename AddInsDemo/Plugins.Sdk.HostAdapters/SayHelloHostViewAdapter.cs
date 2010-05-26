using System.AddIn.Pipeline;
using Plugins.Sdk.Contracts;
using Plugins.Sdk.Views;

namespace Plugins.Sdk.HostAdapters
{
    [HostAdapter]
    public class SayHelloHostViewAdapter : SayHelloHostView
    {
        private ISayHelloAddIn _contract;
        private ContractHandle _handle;

        public SayHelloHostViewAdapter(ISayHelloAddIn contract)
        {
            this._contract = contract;
            _handle = new ContractHandle(contract);
        }

        public override string SayHello(string name)
        {
            return this._contract.SayHello(name);
        }

    }
}
