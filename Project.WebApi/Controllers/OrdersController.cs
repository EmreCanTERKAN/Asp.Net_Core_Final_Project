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
        public async Task<IActionResult> GetAllOrders([FromQuery]int pageNumber = 1, [FromQuery]int pageSize=10)
        {
            var orders = await _orderService.GetAllOrders(pageNumber,pageSize);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if(order is null)
            {
                return NotFound($"Sipariş {orderId} bulunamadı.");
            }

            return Ok(order);
        }
    }
}
