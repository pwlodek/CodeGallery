using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Producer
{
    class TopicSender
    {
        private string _connectionString = "Endpoint=sb://piotrwservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=embSACe0jQiiSwWkAu9kAFcKa2vUZ8tEAUDLWggCCmw=";
        private string _queueName = "testtopic";
        private TopicClient _client;

        public TopicSender()
        {
            // Create namespace client
            NamespaceManager namespaceClient = NamespaceManager.CreateFromConnectionString(_connectionString);
            TopicDescription topicDescription = null;

            foreach (var item in namespaceClient.GetTopics())
            {
                if (item.Path == "maintopic")
                {
                    topicDescription = item;
                    break;
                }
            }

            if (topicDescription == null)
                topicDescription = namespaceClient.CreateTopic("maintopic");

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(_connectionString);
            _client = factory.CreateTopicClient(topicDescription.Path);
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
