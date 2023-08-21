using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Consumer
{
    class EventReceiver
    {
        private string _connectionString = "Endpoint=sb://testeventhub111.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tAjPkAV2gM3/ewR55y7Fm+X+wQMhkB1EDW/7VH5dZ2E=";
        private string _storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=teststorage111piotr;AccountKey=4qMf9Kat9odR+s9GEQsk4QYcHOiRoIRfhKd1BLU/3LZ1egBM4fjIeRysoNC6vbgBHIZi7lZJtbjgDDKE9tKrgg==;EndpointSuffix=core.windows.net";
        private string _eventHubName = "myeventhub";

        EventProcessorHost _eventProcessorHost;

        public EventReceiver()
        {
            // Create namespace client
            NamespaceManager namespaceClient = NamespaceManager.CreateFromConnectionString(_connectionString);
            namespaceClient.CreateEventHubIfNotExists(_eventHubName);
        }

        public void Receive()
        {
            string eventProcessorHostName = Guid.NewGuid().ToString();
            _eventProcessorHost = new EventProcessorHost(eventProcessorHostName, _eventHubName, EventHubConsumerGroup.DefaultGroupName, _connectionString, _storageConnectionString);
            
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            _eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();
        }

        public void Close()
        {
            _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }

    class SimpleEventProcessor : IEventProcessor
    {
        Stopwatch checkpointStopWatch;

        async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason);
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            Console.WriteLine("SimpleEventProcessor initialized.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset);
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                string data = Encoding.UTF8.GetString(eventData.GetBytes());

                Console.WriteLine(string.Format("Message received.  Partition: '{0}', Data: '{1}'",
                    context.Lease.PartitionId, data));
            }

            //Call checkpoint every 5 minutes, so that worker can resume processing from 5 minutes back if it restarts.
            if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                this.checkpointStopWatch.Restart();
            }
        }
    }
}
