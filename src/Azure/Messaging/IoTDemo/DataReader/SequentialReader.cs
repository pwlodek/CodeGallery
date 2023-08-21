using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataReader
{
    class SequentialReader
    {
        string connectionString = "Endpoint=sb://iothub-ns-piotriothu-395646-31511155ec.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=SIWOr3ISDznlrj7Y5DDncZpDO284wvUUTgQ05h+WbFU=";
        string iotHubD2cEndpoint = "piotriothub";
        EventHubClient eventHubClient;

        public void Run()
        {
            Console.WriteLine("Receive messages. Ctrl-C to exit.\n");
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
            {
                EntityPath = iotHubD2cEndpoint
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            var runtimeInfo = eventHubClient.GetRuntimeInformationAsync().Result;
            var d2cPartitions = runtimeInfo.PartitionIds;

            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            var tasks = new List<Task>();
            foreach (string partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            }
            Task.WaitAll(tasks.ToArray());
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.CreateReceiver(PartitionReceiver.DefaultConsumerGroupName, partition, EventPosition.FromStart());
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                var eventDataCollection = await eventHubReceiver.ReceiveAsync(10);
                if (eventDataCollection == null) continue;

                foreach (var eventData in eventDataCollection)
                {
                    string data = Encoding.UTF8.GetString(eventData.Body.Array);
                    Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
                }
            }
        }
    }
}
