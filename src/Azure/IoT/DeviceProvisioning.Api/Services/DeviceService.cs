using DeviceProvisioning.Api.Model;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProvisioning.Api.Services
{
    public interface IDeviceService
    {
        Task<DeviceRegistration> RegisterAsync(Guid id);

        Task<Device> GetDevice(Guid id);

        Task<IEnumerable<Twin>> GetDevices();
    }

    public class DeviceService : IDeviceService
    {
        private RegistryManager _registryManager;
        private string _hubName;

        public DeviceService(IConfiguration cfg)
        {
            var cs = cfg.GetConnectionString("IoT-Hub");
            _hubName = cfg["HubNames:IoT-Hub"];
            _registryManager = RegistryManager.CreateFromConnectionString(cs);
        }

        public async Task<DeviceRegistration> RegisterAsync(Guid id)
        {
            var primaryKey = "cHJpbWFyeTEyMyFAI2toamtkc2hmZHNqZmtzag==";
            var deviceId = id.ToString();
            var existing = await GetDevice(id);
            if (existing is null)
            {
                var device = new Device(deviceId)
                {
                    Authentication = new AuthenticationMechanism()
                    {
                        Type = AuthenticationType.Sas,
                        SymmetricKey = new SymmetricKey()
                        {
                            PrimaryKey = primaryKey,
                            SecondaryKey = "c2Vjb25kYXJ5YnVhaGFoYTEyMzQ1Njc4IUAkIw=="
                        }
                    }
                };

                var registeredDevice = await _registryManager.AddDeviceAsync(device);
            }            

            return new DeviceRegistration
            {
                DeviceId = id,
                HubName = _hubName,
                Key = primaryKey
            };
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await _registryManager.GetDeviceAsync(id.ToString());
            return device;
        }

        public async Task<IEnumerable<Twin>> GetDevices()
        {
            var queryDevices = _registryManager.CreateQuery("select * from devices", 100);
            var twins = await queryDevices.GetNextAsTwinAsync();
            return twins;
        }
    }
}
