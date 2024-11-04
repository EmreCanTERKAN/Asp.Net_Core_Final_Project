using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Business.Operations.Product;
using Project.Business.Operations.Product.Dtos;
using Project.WebApi.ActionFilters;
using Project.WebApi.Models;

namespace Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);
            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("add/Product")]
        [Authorize(Roles = "Admin")]
        [LogProductActionFilter]
        public async Task<IActionResult> AddProduct(ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var addProductDto = new AddProductDto
            {
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
            };

            var result = await _productService.AddProduct(addProductDto);

            if (result.IsSucceed)
            {
                return Ok(addProductDto);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPatch("{id}/Stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductStock(int id, int changeBy)
        {
            var result = await _productService.UpdateProductStock(id, changeBy);

            if (!result.IsSucceed)
            {
                return NotFound();
            }
            else
            {
                _productService.ClearProductCache();
                return Ok($"Ürün Stoğu {changeBy} Adet Olarak Başarıyla Güncellendi");
            }
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result.IsSucceed)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok(result.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            var updateProductDto = new UpdateProductDto
            {
                Id = id,
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
            };
            var result = await _productService.UpdateProduct(updateProductDto);

            if (!result.IsSucceed)
            {
                return NotFound(result.Message);
            }
            else
            {
                _productService.ClearProductCache();
                return await GetProduct(id);
            }
        }

        [HttpPatch("{id}/Price")]
        public async Task<IActionResult> UpdateProductPrice(int id, decimal changeBy)
        {
            var result = await _productService.UpdateProductPrice(id, changeBy);

            if (!result.IsSucceed)
            {
                return NotFound();
            }
            else
            {
                _productService.ClearProductCache();
                return Ok($"Ürünün Fiyatı {changeBy}'Olarak Başarıyla Güncellendi");
            }
        }



    }
}
