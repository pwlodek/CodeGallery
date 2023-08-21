using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedDevice
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "piotriothub.azure-devices.net";
        static string deviceKey = "eVbRh67Tts7SnNNvb7lcrUVlmfo/yTr2ky3bnYT5Y70=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device1", deviceKey), TransportType.Mqtt);
            deviceClient.ProductInfo = "HappyPath_Simulated-CSharp";
            
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            while (true)
            {
                string[] sensors = new[] { "sensor-A", "sensor-B", "sensor-C" };
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;
                string sensor = sensors[rand.Next(sensors.Length)];

                string levelValue;

                if (rand.NextDouble() > 0.7)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        levelValue = "critical";
                    }
                    else
                    {
                        levelValue = "storage";
                    }
                }
                else
                {
                    levelValue = "normal";
                }

                var telemetryDataPoint = new
                {
                    time = DateTime.UtcNow,
                    deviceId = "myFirstDevice",
                    sensor = sensor,
                    temperature = currentTemperature,
                    humidity = currentHumidity,
                    level = levelValue
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                

                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("level", levelValue);

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sent message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
    }
}
