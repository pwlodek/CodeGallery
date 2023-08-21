using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataReader
{
    class Program
    {

        static void Main(string[] args)
        {
            //var reader = new SequentialReader();
            var reader = new SimpleReader();
            reader.Run();
        }
    }
}
