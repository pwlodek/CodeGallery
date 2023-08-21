using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Producer
{
    class EventSender
    {
        private string _connectionString = "Endpoint=sb://testeventhub111.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tAjPkAV2gM3/ewR55y7Fm+X+wQMhkB1EDW/7VH5dZ2E=";
        private string _hubName = "myeventhub";
        private EventHubClient _client;

        public EventSender()
        {
            _client = EventHubClient.CreateFromConnectionString(_connectionString, _hubName);
        }

        public void Send(string message)
        {
            for (int i = 0; i < 100; i++)
            {
                var data = Encoding.UTF8.GetBytes($"Mesage: {message}, count={i}, date={DateTime.Now}");
                _client.Send(new EventData(data));
            }
        }

        public void Close()
        {
            _client.Close();
        }
    }
}
