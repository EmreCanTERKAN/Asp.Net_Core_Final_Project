using Microsoft.EntityFrameworkCore;
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

        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
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
            var products = await _repository.GetAll()
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity
                }).ToListAsync();

            return products;
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
