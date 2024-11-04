using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Project.Business.Operations.Product;

namespace Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IProductService _productServicec;

        public CacheController(IMemoryCache cache, IProductService productServicec)
        {
            _cache = cache;
            _productServicec = productServicec;
        }

        [HttpPost("Clear-Cache")]
        public IActionResult ClearCache()
        {
            _productServicec.ClearProductCache();
            return Ok();
        }
    }
}
