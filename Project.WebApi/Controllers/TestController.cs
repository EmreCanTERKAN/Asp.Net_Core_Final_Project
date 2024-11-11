using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.MVC.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : Controller
    {
        // Bu endpoint'e sadece Authenticate olmuş kullanıcılar erişebilir
        [Authorize]
        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            return Ok("Token geçerli, kullanıcı oturum açmış.");
        }

        // Bu endpoint'e sadece Admin rolüne sahip kullanıcılar erişebilir
        [Authorize(Roles = "Admin")]
        [HttpGet("check-admin")]
        public IActionResult CheckAdminRole()
        {
            return Ok("Admin rolüne sahip token doğrulandı!");
        }

        // Kullanıcı bilgilerini ve rollerini görmek için
        [Authorize]
        [HttpGet("claims")]
        public IActionResult GetUserClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }

        [HttpGet("throw")]
        public IActionResult ThrowException()
        {
            throw new Exception("Bu bir test istisnasıdır");
        }
    }
}
