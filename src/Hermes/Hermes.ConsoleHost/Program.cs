using System;
using System.Net.Mqtt;
using System.Threading;

namespace Hermes.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("MQTT Host v1.0");

            IMqttServer server = MqttServer.Create(new MqttConfiguration());
            server.ClientConnected += Server_ClientConnected;
            server.Start();

            System.Console.WriteLine("Server started.");
            Thread.Sleep(int.MaxValue);
        }

        private static void Server_ClientConnected(object sender, string e)
        {
            System.Console.WriteLine("Client connected: " + e);
        }
    }
}
