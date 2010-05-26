using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Pipeline;
using System.AddIn.Contract;

namespace Plugins.Sdk
{
    [AddInContract]
    public interface ISayHelloAddIn : IContract
    {
        string SayHello(string name);
    }
}
