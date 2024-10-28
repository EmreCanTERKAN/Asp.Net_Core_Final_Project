using Project.Business.Operations.Order.Dto;

namespace Project.WebApi.Models
{
    public class UpdateOrderRequest
    {
        public List<OrderProductDto> Products { get; set; }
    }
}
