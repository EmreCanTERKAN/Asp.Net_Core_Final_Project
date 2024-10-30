using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Models;
using System.IdentityModel.Tokens.Jwt;
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

            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                MustRevalidate = true,
            };

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var email = jwtToken?.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;

            ViewBag.UserEmail = email;

            var response = await client.GetAsync("Products/GetAllProduct");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();   
                
                var activeProducts = products.Where(p => p.IsDeleted == false).ToList();
                return View(activeProducts);
            }

            return View(new List<ProductDto>()); // Boş bir liste döndür
        }

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
                return RedirectToAction("Index","Product");
            }

            return View("Error", new ErrorViewModel { RequestId = "Unable to delete product." });
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> LoadProducts()
        //{
        //    var client = _clientFactory.CreateClient("ApiClient");

        //    // Token'ı Cookie'den al
        //    var token = Request.Cookies["AuthToken"];
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return Unauthorized(); // Eğer token yoksa yetkisiz hatası döndür
        //    }

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var response = await client.GetAsync("Products/GetAllProduct");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        //        return Json(products);
        //    }

        //    return Unauthorized();
        //}
    }
}
