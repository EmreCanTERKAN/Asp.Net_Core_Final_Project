using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Models;
using System.Net.Http.Headers;

namespace Project.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("ApiClient");

            // Token'ı Cookie'den al
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("Products/GetAllProduct");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
                return View(products);
            }

            return View(new List<ProductDto>()); // Boş bir liste döndür
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            var client = _clientFactory.CreateClient("ApiClient");

            // Token'ı Cookie'den al
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"Products/{productId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error", new ErrorViewModel { RequestId = "Unable to delete product." });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LoadProducts()
        {
            var client = _clientFactory.CreateClient("ApiClient");

            // Token'ı Cookie'den al
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(); // Eğer token yoksa yetkisiz hatası döndür
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("Products/GetAllProduct");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
                return Json(products);
            }

            return Unauthorized();
        }
    }
}
