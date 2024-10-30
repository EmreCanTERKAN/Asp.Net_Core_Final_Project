using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Business.Operations.Order;
using Project.Business.Operations.Order.Dto;
using Project.WebApi.Models;

namespace Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddOrder(AddOrderRequest orderRequest)
        {
            var userIdClaim = User.FindFirst("Id");
            if (userIdClaim is null)
            {
                return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
            }

            int userId = int.Parse(userIdClaim.Value);

            var orderDto = new AddOrderDto
            {
                UserId = userId,
                OrderProducts = orderRequest.OrderProducts.Select(op => new OrderProductDto
                {
                    
                    ProductId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList()
            };

            var result = await _orderService.AddOrder(orderDto);
            if (!result.IsSucceed)
            {
                return BadRequest();
            }
            return Ok(result.Message);
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService.GetAllOrders(pageNumber, pageSize);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (!order.IsSucceed)
            {
                return NotFound($"Sipariş {orderId} bulunamadı.");
            }

            return Ok(order.Data);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] UpdateOrderRequest request)
        {
            if (request is null || request.Products is null || !request.Products.Any())
            {
                return BadRequest("Güncellenecek ürün bilgileri eksik.");
            }

            var orderDto = new UpdateOrderDto
            {
                Products = request.Products,
            };

            var result = await _orderService.UpdateOrderProduts(orderId, orderDto);

            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> SoftDeleteOrder(int orderId)
        {
            var result = await _orderService.SoftDeleteOrder(orderId);
            return result.IsSucceed ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
