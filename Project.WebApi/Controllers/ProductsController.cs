﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Business.Operations.Product;
using Project.Business.Operations.Product.Dtos;
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                return Ok(result);
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
                return Ok($"Ürün Stoğu {changeBy} Adet Olarak Başarıyla Güncellendi");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct (int id)
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

    }
}
