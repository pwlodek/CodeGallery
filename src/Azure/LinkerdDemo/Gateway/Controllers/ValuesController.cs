using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ValuesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"http://backend-svc/api/values");
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                var array = JsonConvert.DeserializeObject<string[]>(body);
                
                return Ok(new { sender = "gateway", body = array });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://backend-svc/api/values/{id}");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();

            return body;
        }
    }
}
