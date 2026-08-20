using DB.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.DAL.Configurations
{
    /// <summary>
    /// Описание полей для OrderItemEnity
    /// </summary>
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEnity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEnity> builder)
        {
            builder.ToTable("order_item");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.IdMenuItem)
                .HasColumnName("id_menu_item");

            builder.Property(x => x.Quantity)
              .HasColumnName("quantity");

            builder.Property(x => x.IdOrder)
                .HasColumnName("id_order");

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.IdOrder)
                .OnDelete(DeleteBehavior.Cascade); // при удалении Order удалятся OrderItem

            builder.HasOne(x => x.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.IdMenuItem);
        }
    }
}
