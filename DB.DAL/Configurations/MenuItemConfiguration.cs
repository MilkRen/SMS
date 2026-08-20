using DB.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.DAL.Configurations
{
    /// <summary>
    /// Описание полей для MenuItemEntity
    /// </summary>
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItemEntity>
    {
        public void Configure(EntityTypeBuilder<MenuItemEntity> builder)
        {
            builder.ToTable("menu_item");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Article)
                .HasColumnName("article")
                .IsRequired(); // не может быть пустым

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("price");

            builder.Property(x => x.IsWeighted)
                .HasColumnName("is_weighted");

            builder.Property(x => x.FullPath)
                .HasColumnName("full_path")
                .IsRequired();

            builder.Property(x => x.Barcodes)
                .HasColumnName("barcodes")
                .HasColumnType("text[]") //  native array для postgresql
                .IsRequired();
        }
    }
}
