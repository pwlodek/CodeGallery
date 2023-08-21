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
    class Subscription
    {
        private string _connectionString = "Endpoint=sb://piotrwservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=embSACe0jQiiSwWkAu9kAFcKa2vUZ8tEAUDLWggCCmw=";
        private string _topicPath = "maintopic";
        private string _subscriptionName = "subscription";
        private SubscriptionClient _client;

        public Subscription(int num)
        {
            _subscriptionName = $"subscription{num}";

            // Create namespace client
            NamespaceManager namespaceClient = NamespaceManager.CreateFromConnectionString(_connectionString);
            SubscriptionDescription subscriptionDescription = null;

            foreach (var item in namespaceClient.GetSubscriptions(_topicPath))
            {
                if (item.Name == _subscriptionName)
                {
                    subscriptionDescription = item;
                    break;
                }
            }

            if (subscriptionDescription == null)
                subscriptionDescription = namespaceClient.CreateSubscription(_topicPath, _subscriptionName);

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(_connectionString);
            _client = factory.CreateSubscriptionClient(_topicPath, _subscriptionName, ReceiveMode.PeekLock);

            //_client = SubscriptionClient.CreateFromConnectionString(_connectionString, _topicPath, _subscriptionName, ReceiveMode.PeekLock);            
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
