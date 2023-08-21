using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Producer
{
    class Sender
    {
        private string _connectionString = "Endpoint=sb://testnamespacepwlodek.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jWa75vqk1yWscjFcQPkxflAMyfhIoH89S5HZZnaVfhQ=";
        private string _queueName = "mainqueue";
        private QueueClient _client;

        public Sender()
        {          

            _client = QueueClient.CreateFromConnectionString(_connectionString, _queueName);
            
        }

        public void Send(string message)
        {
            var msg = new BrokeredMessage(message);
            _client.Send(msg);
        }

        public void Close()
        {
            _client.Close();
        }
    }
}
