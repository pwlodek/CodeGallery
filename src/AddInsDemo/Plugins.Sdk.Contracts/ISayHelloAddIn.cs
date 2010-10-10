using System;
using System.AddIn.Pipeline;
using System.AddIn.Contract;

namespace Plugins.Sdk.Contracts
{
    [AddInContract]
    public interface ISayHelloAddIn : IContract
    {
        string SayHello(string name);
    }
}
