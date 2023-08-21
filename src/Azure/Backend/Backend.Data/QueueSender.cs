using Backend.Data.Model;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class QueueSender
    {
        private string _connectionString = "Endpoint=sb://testservicebuspiotr.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=vgO2vkNXfcwz1/3wI2zIR96Qz2eOKU77GP1k7rI3uFI=";
        private string _queueName = "mainqueue";
        private QueueClient _client;

        public QueueSender()
        {
            _client = QueueClient.CreateFromConnectionString(_connectionString, _queueName);
        }

        public void Send(Message message)
        {
            Console.WriteLine($"Sending message {message.Item.Name} to the queue");
            var msg = new BrokeredMessage(message);
            _client.Send(msg);
        }

        public void Close()
        {
            _client.Close();
        }
    }
}
