using System;

namespace ConventionCatalogDemo
{
    /********** INTERFACE **********/

    public interface ILogger
    {
        void Log(string message);
    }

    /********** IMPLEMENTATION **********/

    public class LoggerImpl : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}