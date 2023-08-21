using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4net.Azure
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

        private static readonly ILog _logger2 = LogManager.GetLogger("test logger");

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            _logger.Info("logging events");

            for (int i = 0; i < 1000; i++)
            {
                _logger.Warn($"logging events: {i}");
            }

            Task.Run(() => {
                for (int i = 0; i < 1000; i++)
                {
                    _logger.Info($"Thread 1 logging events: {i}");
                }
            });

            Task.Run(() => {
                for (int i = 0; i < 1000; i++)
                {
                    _logger2.Warn($"Thread 2 logging events: {i}");
                }
            });

            //LogManager.Flush(2000);

            Console.WriteLine("Hit enter to quit.");
            Console.ReadKey();
        }
    }
}
