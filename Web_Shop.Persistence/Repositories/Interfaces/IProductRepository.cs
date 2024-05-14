using WWSI_Shop.Persistence.MySQL.Model;
using System.Threading.Tasks;
using System.Net;




namespace Web_Shop.Persistence.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> SkuExistsAsync(string sku);
        Task<bool> IsSkuEditAllowedAsync(string sku, ulong id);
        Task<Product?> GetBySkuAsync(string sku);

        Task<(bool IsSuccess, Product? entity)> AddAndSaveAsync(Product entity);
        Task<(bool IsSuccess, HttpStatusCode StatusCode, string ErrorMessage)> DeleteAndSaveAsync(ulong id);

    }
}
