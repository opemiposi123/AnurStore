using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal class ProductSaleEntityTypeConfiguration : IEntityTypeConfiguration<ProductSale>
    {
        public void Configure(EntityTypeBuilder<ProductSale> builder)
        {
            builder.ToTable("ProductSales");

            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.TotalAmount)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(ps => ps.SaleDate)
                .IsRequired();

            builder.Property(ps => ps.CustomerName)
                .HasMaxLength(100); 

            builder.Property(ps => ps.Discount)
                .HasColumnType("money");

            builder.Property(ps => ps.PaymentMethod)
                .IsRequired(); 

            builder.HasMany(ps => ps.ProductSaleItems)
                .WithOne(psi => psi.ProductSale)
                .HasForeignKey(psi => psi.ProductSaleId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
 