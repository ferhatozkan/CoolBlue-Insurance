using Insurance.Api.Clients.Models;
using System.Threading.Tasks;

namespace Insurance.Api.Clients
{
    public interface IProductApiClient
    {
        Task<ProductTypeDto> GetProductType(int productTypeId);
        Task<ProductDto> GetProduct(int productId);
    }
}
