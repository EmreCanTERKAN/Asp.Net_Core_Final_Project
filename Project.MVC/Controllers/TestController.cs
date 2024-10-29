using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.MVC.Controllers
{
    public class TestController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public TestController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Authorize]
        public async Task<IActionResult> TestAuth()
        {
            var client = _clientFactory.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Unauthorized("Token bulunamadı.");
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("test/check-auth");

            if (response.IsSuccessStatusCode)
            {
                return Ok("Token geçerli ve kullanıcı doğrulandı.");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return Unauthorized($"API doğrulaması başarısız: {content}");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TestAdminRole()
        {
            var client = _clientFactory.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Unauthorized("Token bulunamadı.");
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("test/check-admin");

            if (response.IsSuccessStatusCode)
            {
                return Ok("Admin rolüne sahip token doğrulandı!");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return Unauthorized($"API doğrulaması başarısız: {content}");
            }
        }
    }
}
