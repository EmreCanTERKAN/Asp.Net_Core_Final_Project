using Microsoft.AspNetCore.Mvc;
using Project.MVC.Models;

namespace Project.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var client = _clientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("Auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

                // Cookie'de Token'ı sakla
                Response.Cookies.Append("AuthToken", tokenResponse.Token, new CookieOptions
                {
                    HttpOnly = true, // JavaScript ile erişime kapalı
                    Secure = true, // HTTPS bağlantısında gönderilir
                    Expires = DateTime.UtcNow.AddHours(1) // Token süresine göre ayarlayın
                });

                return RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış");
            return View(loginRequest);
        }

    }
}
