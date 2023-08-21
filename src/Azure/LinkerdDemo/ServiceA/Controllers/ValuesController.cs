using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static Random _rnd = new Random((int)DateTime.Now.Ticks);

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            // 33% of requests should fail
            if (_rnd.NextDouble() < .33) throw new InvalidOperationException();

            return new string[] { "value1 service A", "value2 service A" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            // If you want to test status code returns
            if (id > 1000 && id < 2000)
            {
                var retCode = id - 1000;
                return StatusCode(retCode);
            }

            // 1 in 4 requests should take a second
            if (_rnd.NextDouble() < .25) Thread.Sleep(1000);

            return $"This value comes from service A: {id}";
        }
    }
}
