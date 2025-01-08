using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class ProductEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50); 
            builder.Property(p => p.Description)
                .HasMaxLength(100); 

            builder.Property(p => p.BarCode)
                .HasMaxLength(50); 

            builder.Property(p => p.PricePerPack)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(p => p.PackPriceMarkup)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2");

            builder.Property(p => p.UnitPrice)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(p => p.UnitPriceMarkup)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2");

            builder.Property(p => p.ProductImageUrl)
                .HasMaxLength(200);

            builder.Property(p => p.TotalItemInPack)
                .IsRequired();

            builder.Property(p => p.CategoryId)
                .IsRequired();

            builder.Property(p => p.BrandId)
                .IsRequired(false); 
            
            builder.HasOne(p => p.Inventory)
                .WithOne(i => i.Product) 
                .HasForeignKey<Inventory>(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.ProductSize)
                .IsRequired(); 
        }
    }
}
