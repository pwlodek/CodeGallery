using System;
using System.AddIn.Hosting;
using System.Collections.ObjectModel;
using Plugins.Sdk.Views;

namespace AddInHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the add-ins discovery root directory to be the current directory
            string addinRoot = Environment.CurrentDirectory;

            // Rebuild the add-ins cache and pipeline components cache.
            AddInStore.Rebuild(addinRoot);

            // Get registerd add-ins of type SimpleAddInHostView
            Collection<AddInToken> addins = AddInStore.FindAddIns(typeof(SayHelloHostView), addinRoot);

            Console.WriteLine("Main AppDomain: " + AppDomain.CurrentDomain.FriendlyName);

            AddInProcess process = new AddInProcess();
            process.Start();

            Console.WriteLine("External AddIn ProcessID=" + process.ProcessId);
            
            foreach (AddInToken addinToken in addins)
            {
                // Activate the add-in
                //SayHelloHostView addinInstance = addinToken.Activate<SayHelloHostView>(AddInSecurityLevel.Host);
                SayHelloHostView addinInstance = addinToken.Activate<SayHelloHostView>(process, AddInSecurityLevel.Host);

                // Use the add-in
                Console.WriteLine();
                Console.WriteLine(String.Format("Add-in {0} Version {1}",
                     addinToken.Name, addinToken.Version));

                Console.WriteLine(addinInstance.SayHello("Hello World from Plugin ;-)"));
            }

            Console.ReadKey();
        }
    }
}
