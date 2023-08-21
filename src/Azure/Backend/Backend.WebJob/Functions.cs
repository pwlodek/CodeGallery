using Backend.Data;
using Backend.Data.Model;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.WebJob
{
    public class Functions
    {
        private CosmosDbProvider _provider;

        public Functions()
        {
            _provider = new CosmosDbProvider();
        }

        public void SaveTodoItem([ServiceBusTrigger("mainqueue")] Message msg, TextWriter logger)
        {
            logger.WriteLine("Queue message refers to todo item: " + msg.Item.Name);

            _provider.AddTodoItem(msg.Item);
        }
    }
}
