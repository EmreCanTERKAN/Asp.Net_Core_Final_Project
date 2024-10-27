﻿using System.ComponentModel.DataAnnotations;

namespace Project.WebApi.Models
{
    public class UpdateProductRequest
    {
        [Required]       
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}