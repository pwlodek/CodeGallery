using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace Consumer
{
    public class Program
    {
        private const string EhConnectionString = "Endpoint=sb://pwlodek-ceres-eventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LBq03zdOdRlWOFpamVbAZ5GTSPNOrHH61PCQayjCn78=";
        private const string EhEntityPath = "test";
        private const string StorageContainerName = "leases";
        private const string StorageAccountName = "pwlodekceresstorage";
        private const string StorageAccountKey = "PyEK0x9XeXmeCvhuGFtxaGBN7BvvyQQOc5D5vabIh1opzc8xw9mmxYfJa8eSsJsCpSL2CTEOHAG3ce4+tOrIxA==";

        //DefaultEndpointsProtocol=https;AccountName=pwlodekceresstorage;AccountKey=PyEK0x9XeXmeCvhuGFtxaGBN7BvvyQQOc5D5vabIh1opzc8xw9mmxYfJa8eSsJsCpSL2CTEOHAG3ce4+tOrIxA==;EndpointSuffix=core.windows.net
        private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
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
