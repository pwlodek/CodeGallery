using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using ECommerce.Domain.Services;
using ECommerce.Domain;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductCatalogService productCatalogService;

        public ProductsController()
        {
            this.productCatalogService = ServiceProxy.Create<IProductCatalogService>(
                new Uri("fabric:/ECommerce/ECommerce.ProductCatalog"),
                new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(0));
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<ProductModel>> Get()
        {
            try
            {
                var products = await productCatalogService.GetAllProducts();
                return products.Select(t => new ProductModel { Id = t.Id, Description = t.Description, Name = t.Name, Price = t.Price, IsAvailability = t.Availability > 0 });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]ProductModel product)
        {
            var p = new Product { Id = product.Id, Name = product.Name, Description = product.Description, Price = product.Price };
            await this.productCatalogService.AddProduct(p);
        }
    }
}
