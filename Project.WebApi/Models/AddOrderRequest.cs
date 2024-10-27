namespace Project.WebApi.Models
{
    public class AddOrderRequest
    {
        public int UserId { get; set; }
        public List<OrderProductRequest> OrderProducts { get; set; }
    }
}
