using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project.Business.Operations.Product.Dtos;
using Project.Business.Types;
using Project.Data.Entities;
using Project.Data.Repositories;
using Project.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Product
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductEntity> _repository;
        private readonly IMemoryCache _cache;

        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _cache = cache;
        }
        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            var hasProduct = _repository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower()).Any();

            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün kayıtlarda mevcut, başka bir ürün deneyiniz."
                };
            }

            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
            };

            await _repository.Add(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Kayıt sırasında bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
            };


        }
        public async Task<ServiceMessage> DeleteProduct(int id)
        {
            var product = _repository.GetById(id);

            if (product is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün Bulunamadı"
                };
            }

            product.IsDeleted = true;

            await _repository.Update(product);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Silme işlemi sırasında bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün Başarılı Şekilde Silinmiştir"
            };
        }
        public async Task<List<ProductDto>> GetAllProducts()
        {
            //cache
            if (!_cache.TryGetValue("ProductsCache",out List<ProductDto> cachedProducts))
            {
                var products = await _repository.GetAll()
               .Select(x => new ProductDto
               {
                   Id = x.Id,
                   ProductName = x.ProductName,
                   Price = x.Price,
                   StockQuantity = x.StockQuantity
               }).ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set("ProductsCache", products, cacheEntryOptions);

                cachedProducts = products;
            }

            return cachedProducts!;
        }
        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _repository.GetAll(x => x.Id == id)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity
                }).FirstOrDefaultAsync();
            return product!;
        }
        public async Task<ServiceMessage> UpdateProduct(UpdateProductDto product)
        {
            var productEntity = _repository.GetById(product.Id);
            if (productEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Ürün Bulunamadı"
                };
            }

            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;
            productEntity.StockQuantity = product.StockQuantity;

            await _repository.Update(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Bir hata ile karşılaşıldı.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "İşlem Başarılı."
            };

        }
        public async Task<ServiceMessage> UpdateProductStock(int id, int changeBy)
        {
            var product = _repository.GetById(id);
            if (product is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün Bulunamadı"
                };
            }

            product.StockQuantity = changeBy;

            await _repository.Update(product);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true

            };
        }

    }
}
