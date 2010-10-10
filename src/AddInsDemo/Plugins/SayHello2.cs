using System;
using System.AddIn;
using Plugins.Sdk.Views;

namespace Plugins
{
    [AddIn("My Second Add-In", Version = "1.0.0.0", Description = "Description of My First Add-In", Publisher = "AGH Univ.")]
    public class SayHelloImpl2 : SayHelloAddinView
    {
        public override string SayHello(string name)
        {
            Console.WriteLine("Plugin AppDomain: " + AppDomain.CurrentDomain.FriendlyName);
            return "Plugin2: " + name;
        }
    }
}
