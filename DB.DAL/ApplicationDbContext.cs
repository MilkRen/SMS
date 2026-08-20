using DB.DAL.Configurations;
using DB.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DB.DAL
{
    /// <summary>
    /// Настройки БД
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<MenuItemEntity> MenuItem { get; set; }

        public DbSet<OrderEntity> Order { get; set; }

        public DbSet<OrderItemEnity> OrderItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new MenuItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
