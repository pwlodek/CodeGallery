using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Consumer
{
    class Receiver
    {
        private string _connectionString = "Endpoint=sb://testnamespacepwlodek.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jWa75vqk1yWscjFcQPkxflAMyfhIoH89S5HZZnaVfhQ=";
        private string _queueName = "mainqueue";
        private QueueClient _client;

        public Receiver()
        {
            _client = QueueClient.CreateFromConnectionString(_connectionString, _queueName, ReceiveMode.PeekLock);
            
        }

        public void Receive()
        {
            _client.OnMessage(m =>
            {
                Console.WriteLine("Message body: " + m.GetBody<String>());
                Console.WriteLine("Message id: " + m.MessageId);

                _client.Complete(m.LockToken);

                Thread.Sleep(1000);
            }, new OnMessageOptions() { AutoComplete = false });
        }

        public void Close()
        {
            _client.Close();
        }
    }
}
