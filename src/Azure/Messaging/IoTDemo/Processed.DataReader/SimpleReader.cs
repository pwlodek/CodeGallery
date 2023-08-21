using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Processed.DataReader
{
    class SimpleReader
    {
        private const string EhConnectionString = "Endpoint=sb://piotrnext.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=6osbAHTtIoKEWBuEiw6BfeM4E46CwY0unwRCO6+/M6g=";
        private const string EhEntityPath = "avgevents";
        private const string StorageContainerName = "leases3";
        private const string StorageAccountName = "pwlodekceresstorage";
        private const string StorageAccountKey = "PyEK0x9XeXmeCvhuGFtxaGBN7BvvyQQOc5D5vabIh1opzc8xw9mmxYfJa8eSsJsCpSL2CTEOHAG3ce4+tOrIxA==";

        //DefaultEndpointsProtocol=https;AccountName=pwlodekceresstorage;AccountKey=PyEK0x9XeXmeCvhuGFtxaGBN7BvvyQQOc5D5vabIh1opzc8xw9mmxYfJa8eSsJsCpSL2CTEOHAG3ce4+tOrIxA==;EndpointSuffix=core.windows.net
        private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        public void Run()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private async Task RunAsync()
        {
            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                EhEntityPath,
                PartitionReceiver.DefaultConsumerGroupName,
                EhConnectionString,
                StorageConnectionString,
                StorageContainerName);

            var eventProcessorOptions = new EventProcessorOptions();
            eventProcessorOptions.SetExceptionHandler(e => {
                Console.WriteLine("Error: " + e.Exception);
            });

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(eventProcessorOptions);

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}
