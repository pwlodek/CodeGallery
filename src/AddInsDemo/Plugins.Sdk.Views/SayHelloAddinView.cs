using System;
using System.AddIn.Pipeline;

namespace Plugins.Sdk.Views
{
    [AddInBase]
    public abstract class SayHelloAddinView
    {
        public abstract string SayHello(string name);
    }
}
