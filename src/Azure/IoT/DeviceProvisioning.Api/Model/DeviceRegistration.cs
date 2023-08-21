using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProvisioning.Api.Model
{
    public class DeviceRegistration
    {
        public Guid DeviceId { get; set; }

        public string Key { get; set; }

        public string HubName { get; set; }
    }
}
