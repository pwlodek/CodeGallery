using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplicationWithMetadata.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationWithMetadata.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        // GET: api/products
        /// <summary>
        /// Gets a list of products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return new Product[] { new Product { Name = "Carrot", Price = 123 },new Product { Name = "Paprika", Price = 10 } };
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return Get().First();
        }

        // POST api/products
        [HttpPost]
        public void Post([FromBody]Product value)
        {
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Product value)
        {
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
