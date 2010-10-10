using System;
using System.AddIn;
using System.Diagnostics;
using Plugins.Sdk.Views;

namespace Plugins
{
    [AddIn("My First Add-In", Version = "1.0.0.0", Description = "Description of My First Add-In", Publisher = "AGH Univ.")]
    public class SayHelloImpl1 : SayHelloAddinView
    {
        public override string SayHello(string name)
        {
            Console.WriteLine("Plugin AppDomain: " + AppDomain.CurrentDomain.FriendlyName);
            return "Plugin1: " + name + "; ProcessID=" + Process.GetCurrentProcess().Id;
        }
    }
}
