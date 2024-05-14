using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Extensions;
using Web_Shop.Application.Helpers.PagedList;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Persistence.Repositories.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using WWSI_Shop.Persistence.MySQL.Model;



using BC = BCrypt.Net.BCrypt;

namespace Web_Shop.Application.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(ILogger<Product> logger,
                              ISieveProcessor sieveProcessor,
                              IOptions<SieveOptions> sieveOptions,
                              IUnitOfWork unitOfWork)
                             // IProductRepository productRepository)
            : base(logger, sieveProcessor, sieveOptions, unitOfWork)
        {
            //_productRepository = productRepository;
        }

        public async Task<(bool IsSuccess, HttpStatusCode StatusCode, string ErrorMessage)> DeleteProductAsync(ulong id)
        {
            try
            {
                var result = await _productRepository.DeleteAndSaveAsync(id);

                if (!result.IsSuccess)
                {
                    return (false, result.StatusCode, result.ErrorMessage);
                }

                return (true, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewProductAsync(AddUpdateProductDTO dto)
        {
            try
            {
                if (await _unitOfWork.ProductRepository.SkuExistsAsync(dto.Sku))
                {
                    return (false, default(Product), HttpStatusCode.BadRequest, "this product sku: " + dto.Sku + " exist.");
                }
                var newEntity = dto.MapProduct();
                var result = await AddAndSaveAsync(newEntity);
                return (true, result.entity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }



        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingProductAsync(AddUpdateProductDTO dto, ulong id)
        {
            try
            {
                var existingEntityResult = await WithoutTracking().GetByIdAsync(id);

                if (!existingEntityResult.IsSuccess)
                {
                    return existingEntityResult;
                }


                var domainEntity = dto.MapProduct();
                domainEntity.IdProduct = id;

                var result = await UpdateAndSaveAsync(domainEntity, id);

               // return (true, result.entity, HttpStatusCode.OK, string.Empty);
                return await UpdateAndSaveAsync(domainEntity, id);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IPagedList<Product, GetSingleProductDTO>? entityList, HttpStatusCode StatusCode, string ErrorMessage)> SearchProductsAsync(SieveModel paginationParams)
        {
            try
            {
                var query = _productRepository.Entities.AsNoTracking();

                var result = await query.ToPagedListAsync(_sieveProcessor,
                                                          _sieveOptions,
                                                          paginationParams,
                                                          formatterCallback => DomainToDtoMapper.MapGetSingleProductDTO(formatterCallback));

                return (true, result, HttpStatusCode.OK, String.Empty);
            }
            catch (Exception ex)
            {
                var error = LogError(ex.Message);

                return (false, default, error.StatusCode, error.ErrorMessage);
            }
        }

        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> GetProductByIdAsync(ulong id)
        {
            try
            {
                var result = await GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    return result;
                }

                return (true, result.entity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

       
    }
}
