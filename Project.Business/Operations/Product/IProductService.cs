using Project.Business.Operations.Product.Dtos;
using Project.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.Product
{
    public interface IProductService
    {
        Task<ServiceMessage> AddProduct(AddProductDto product);

        Task<ServiceMessage> UpdateProductStock(int id, int changeBy);

        Task<ServiceMessage> DeleteProduct(int id);

        Task<List<ProductDto>> GetAllProducts();
        
        Task<ProductDto> GetProduct(int id);

        Task<ServiceMessage> UpdateProduct(UpdateProductDto product);
    }
}
