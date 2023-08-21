using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceProvisioning.Api.Model;
using DeviceProvisioning.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace DeviceProvisioning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Twin>>> Get()
        {
            return Ok(await _deviceService.GetDevices());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> Get(Guid id)
        {
            return Ok(await _deviceService.GetDevice(id));
        }

        // POST api/values
        [HttpPost("register/{id:Guid}")]
        public async Task<ActionResult<DeviceRegistration>> RegisterDevice([FromRoute] Guid id)
        {
            var registration = await _deviceService.RegisterAsync(id);
            return Ok(registration);
        }
    }
}
