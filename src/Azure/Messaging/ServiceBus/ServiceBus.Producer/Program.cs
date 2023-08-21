using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var sender = new EventSender();

            for (int i = 0; i < 100; i++)
            {
                var msg = "Important message: " + i;
                Console.WriteLine(msg);
                sender.Send(msg);

                Thread.Sleep(500);
            }

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}
