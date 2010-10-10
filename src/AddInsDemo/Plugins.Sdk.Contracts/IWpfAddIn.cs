using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Plugins.Sdk.Contracts
{
    [AddInContract]
    public interface IWpfAddIn : IContract
    {
        INativeHandleContract RegisterContent();
    }
}
