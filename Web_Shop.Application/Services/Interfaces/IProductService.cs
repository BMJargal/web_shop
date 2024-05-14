using Sieve.Models;
using System.Net;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Helpers.PagedList;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Services.Interfaces
{
    public interface IProductService : IBaseService<Product>
    {
        Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewProductAsync(AddUpdateProductDTO dto);
        Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingProductAsync(AddUpdateProductDTO dto, ulong id);
        Task<(bool IsSuccess, IPagedList<Product, GetSingleProductDTO>? entityList, HttpStatusCode StatusCode, string ErrorMessage)> SearchProductsAsync(SieveModel paginationParams);
        Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> GetProductByIdAsync(ulong id);
        Task<(bool IsSuccess, HttpStatusCode StatusCode, string ErrorMessage)> DeleteProductAsync(ulong id);
    }
}
