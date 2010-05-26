using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello from managed code!");
            Console.WriteLine("\tAppDomain Name: " + AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("\tIs default AppDomain: " + AppDomain.CurrentDomain.IsDefaultAppDomain());
            Console.WriteLine("\tCLR Version: " + Environment.Version);
            Console.WriteLine("\tCPUs: " + Environment.ProcessorCount);
            Console.WriteLine("\tOS: " + Environment.OSVersion);
            Console.WriteLine("\tGC Server: " + System.Runtime.GCSettings.IsServerGC);
            Console.WriteLine("\tCollection Count: " + GC.CollectionCount(0));
        }

        private static int _Main(string args)
        {
            Main(args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            return 0;
        }
    }
}
