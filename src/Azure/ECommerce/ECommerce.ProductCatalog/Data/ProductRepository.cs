using ECommerce.Domain;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalog.Data
{
    class ProductRepository
    {
        private readonly IReliableStateManager stateManager;

        public ProductRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            var list = new List<Product>();

            using (var t = stateManager.CreateTransaction())
            {
                var e = await products.CreateEnumerableAsync(t);
                using (var en = e.GetAsyncEnumerator())
                {
                    while (await en.MoveNextAsync(CancellationToken.None))
                    {
                        var p = en.Current.Value;
                        list.Add(p);
                    }
                }
                await t.CommitAsync();
            }

            return list;
        }

        public async Task AddProduct(Product p)
        {
            var products = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

            using (var t = stateManager.CreateTransaction())
            {
                await products.AddAsync(t, p.Id, p);
                await t.CommitAsync();
            }
        }
    }
}
