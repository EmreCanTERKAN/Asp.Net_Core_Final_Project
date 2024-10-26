using Microsoft.EntityFrameworkCore;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Context
{
    public class FinalProjectDbContext : DbContext
    {
        public FinalProjectDbContext(DbContextOptions<FinalProjectDbContext> options) : base(options)
        {

        }

        //FluentApi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderProductEntity> OrderProductEntities => Set<OrderProductEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();



    }
}
