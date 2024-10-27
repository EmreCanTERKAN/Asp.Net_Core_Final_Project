using System.ComponentModel.DataAnnotations;

namespace Project.WebApi.Models
{
    public class ProductRequest
    {
        [Required]
        [Length(5,50)]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}
