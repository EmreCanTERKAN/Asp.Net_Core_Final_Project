using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Foreing Key
        public int UserId { get; set; }


        //Relation
        public UserEntity User { get; set; }
        public ICollection<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();

    }

    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            base.Configure(builder);
        }
    }
}
