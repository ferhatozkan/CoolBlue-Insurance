using System.Threading.Tasks;
using Insurance.Api.Infrastructure.Clients.Models;

namespace Insurance.Api.Application.Clients
{
    public interface IProductApiClient
    {
        Task<ProductType> GetProductType(int productTypeId);
        Task<Product> GetProduct(int productId);
    }
}
