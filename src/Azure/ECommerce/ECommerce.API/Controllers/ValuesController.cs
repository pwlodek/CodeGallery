using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors;
using SampleActor.Interfaces;
using System.Threading;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private ActorId _actorId = ActorId.CreateRandom();

        public ValuesController()
        {
            
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            ISampleActor myActor = ActorProxy.Create<ISampleActor>(_actorId, new Uri("fabric:/ECommerce/SampleActorService"));

            return await myActor.GetValuesAsync(CancellationToken.None);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
