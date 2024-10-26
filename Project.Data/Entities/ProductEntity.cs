using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class ProductEntity  :BaseEntity
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        //Relational Property
        public ICollection<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();
    }

    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            // decimal değere basamak ve hassasiyet belirttim. Parasal verilerde virgülden sonra 2 basamak en yaygın yöntemdir.
            builder.Property(e => e.Price).HasColumnType("decimal(18,2)");
            base.Configure(builder);
        }
    }
}
