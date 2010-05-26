using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Pipeline;

namespace Plugins.Sdk
{
    [AddInBase]
    public abstract class SayHelloAddinView
    {
        public abstract string SayHello(string name);
    }
}
