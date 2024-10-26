using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; } // şifreleme olacak.
        public UserType UserType { get; set; }

        // Relational Property
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }

    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
