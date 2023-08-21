using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Backend.Data;
using Backend.Data.Model;

namespace Backend.FunctionsApp
{
    public class Function1
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("mainqueue", AccessRights.Manage, Connection = "")]Message msg, TraceWriter log)
        {
            log.Info("Queue message refers to todo item: " + msg.Item.Name);

            var provider = new CosmosDbProvider();
            provider.AddTodoItem(msg.Item);
        }
    }
}
