using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Services
{
    public interface IProductCatalogService : IService
    {

        Task<IEnumerable<Product>> GetAllProducts();

        Task AddProduct(Product p);
    }
}
