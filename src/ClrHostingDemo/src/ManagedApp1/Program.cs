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
            Console.WriteLine("\tCLR Version: " + Environment.Version);
            Console.WriteLine("\tCPUs: " + Environment.ProcessorCount);
            Console.WriteLine("\tOS: " + Environment.OSVersion);
            Console.WriteLine("\tGC Server: " + System.Runtime.GCSettings.IsServerGC);
        }

        private static int _Main(string args)
        {
            Main(args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            return 0;
        }
    }
}
