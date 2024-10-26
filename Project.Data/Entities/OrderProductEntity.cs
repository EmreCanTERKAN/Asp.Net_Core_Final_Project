using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class OrderProductEntity : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        //Relation

        public OrderEntity Order { get; set; }
        public ProductEntity Product { get; set; }
    }

    public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            //baseden gelen idyi yoksaydık.
            builder.Ignore(x => x.Id);
            // Primary key olarak orderId ile Product idyi verdik
            builder.HasKey("OrderId", "ProductId");
            base.Configure(builder);
        }
    }
}
