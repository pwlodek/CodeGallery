using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherStationSimulatedDevice
{
    class Program
    {
        private static DeviceClient s_deviceClient;

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync()
        {
            // Initial telemetry values
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                // Create JSON message
                var telemetryDataPoint = new
                {
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                // Send the telemetry message
                await s_deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
        private static void Main(string[] args)
        {
            Console.WriteLine("IoT Hub Quickstarts #1 - Simulated device. Ctrl-C to exit.\n");

            var deviceId = Guid.NewGuid();

            var hc = new HttpClient();
            var result = hc.PostAsync($"http://localhost:5000/api/devices/register/{deviceId}", new StringContent(string.Empty)).Result;
            result.EnsureSuccessStatusCode();


            var registrationText = result.Content.ReadAsStringAsync().Result;
            var dr = JsonConvert.DeserializeObject<DeviceRegistration>(registrationText);

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString($"HostName={dr.HubName}.azure-devices.net;DeviceId={dr.DeviceId};SharedAccessKey={dr.Key}", TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }

    public class DeviceRegistration
    {
        public Guid DeviceId { get; set; }

        public string Key { get; set; }

        public string HubName { get; set; }
    }
}
