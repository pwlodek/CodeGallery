using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ProductCatalog.Data;
using ECommerce.Domain.Services;
using ECommerce.Domain;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace ECommerce.ProductCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public class ProductCatalog : StatefulService, IProductCatalogService
    {
        private ProductRepository productRepository;

        public ProductCatalog(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddProduct(Product p)
        {
            await productRepository.AddProduct(p);
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return productRepository.GetAllProducts();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[] { new ServiceReplicaListener(ctx => this.CreateServiceRemotingListener(ctx)) };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            productRepository = new ProductRepository(StateManager);

            var allp = await productRepository.GetAllProducts();
            if (allp.Count() == 0)
            {
                var p1 = new Product() { Id = Guid.NewGuid(), Name = "Product 1", Availability = 100, Description = "some desc", Price = 999 };
                var p2 = new Product() { Id = Guid.NewGuid(), Name = "Product 2", Availability = 100, Description = "some desc", Price = 999 };
                var p3 = new Product() { Id = Guid.NewGuid(), Name = "Product 3", Availability = 100, Description = "some desc", Price = 999 };

                await productRepository.AddProduct(p1);
                await productRepository.AddProduct(p2);
                await productRepository.AddProduct(p3);
            }
        }
    }
}
