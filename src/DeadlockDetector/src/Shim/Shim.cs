using System;
using System.Threading;

namespace DeadlockDetector
{
    public class Shim
    {
        private static int Start(string args)
        {
            int exitCode = -1;

            try
            {
                // The input arrives in a single string. Split it up.
                string[] shimArgs = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);                

                string[] _args = new string[shimArgs.Length - 1];
                Array.Copy(shimArgs, 1, _args, 0, _args.Length);

                // Now, execute the program in its own thread.
                AppDomain appDomain = AppDomain.CurrentDomain;

                Thread mainThread = new Thread(delegate()
                {
                    try
                    {
                        exitCode = appDomain.ExecuteAssembly(shimArgs[0], appDomain.Evidence, _args);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Profiled program error: " + ex.Message);
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
    }
}
