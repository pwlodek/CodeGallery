using System;
using System.Reflection;
using System.Security.Policy;
using System.Threading;

namespace DemoHost3
{
    public class Shim
    {
        private static int Start(string args)
        {
            int exitCode = -1;

            try
            {
                Console.WriteLine("Initializing environment...");
                Console.WriteLine("\tAppDomain Name: " + AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine("\tIs Default AppDomain: " + AppDomain.CurrentDomain.IsDefaultAppDomain());
                
                // The input arrives in a single string. Split it up.
                string[] shimArgs = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);                

                string[] _args = new string[shimArgs.Length - 1];
                Array.Copy(shimArgs, 1, _args, 0, _args.Length);

                // Now, execute the program in its own thread and domain.
                AppDomain appDomain = AppDomain.CreateDomain("UserCodeDomain");
                appDomain.AssemblyResolve += new ResolveEventHandler(Shim.AssemblyResolve);

                Thread mainThread = new Thread(delegate()
                {
                    try
                    {
                        Console.WriteLine("Executing user code...");

                        // Execute user assembly.
                        Evidence evidence = new Evidence();
                        exitCode = appDomain.ExecuteAssembly(shimArgs[0], evidence, _args);

                        // Unload the domain.
                        AppDomain.Unload(appDomain);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Program error: " + ex.Message);
                    }
                });

                mainThread.Start();
                mainThread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Shim error:" + ex.Message);
            }

            return exitCode;
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
