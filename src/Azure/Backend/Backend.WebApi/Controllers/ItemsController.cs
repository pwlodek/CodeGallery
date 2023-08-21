using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Data.Model;

namespace Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private CosmosDbProvider _dbProvider;
        private QueueSender _queue;

        public ItemsController()
        {
            _dbProvider = new CosmosDbProvider();
            _queue = new QueueSender();
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<TodoItem>> Get()
        {
            return await _dbProvider.GetTodoItems();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<TodoItem> Get(string id)
        {
            return await _dbProvider.GetTodoItem(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]TodoItem value)
        {
            _queue.Send(new Message { Operation = Operation.Add, Item = value });

            return Ok();
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
