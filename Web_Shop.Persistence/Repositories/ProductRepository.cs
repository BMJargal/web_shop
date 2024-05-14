using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;
using Web_Shop.Persistence.Repositories.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Shop.Persistence.Repositories;
using System.Net;


namespace Web_Shop.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(WwsishopContext context) : base(context) { }

        public async Task<bool> SkuExistsAsync(string sku)
        {
            return await Entities.AnyAsync(p => p.Sku == sku);
        }

        public async Task<bool> IsSkuEditAllowedAsync(string sku, ulong id)
        {
            return !await Entities.AnyAsync(p => p.Sku == sku && p.IdProduct != id);
        }

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            return await Entities.FirstOrDefaultAsync(p => p.Sku == sku);
        }

        public Task<(bool IsSuccess, Product? entity)> AddAndSaveAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<(bool IsSuccess, HttpStatusCode StatusCode, string ErrorMessage)> DeleteAndSaveAsync(ulong id)
        {
            throw new NotImplementedException();
        }
    }
}
