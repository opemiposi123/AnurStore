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

            builder.Property(ps => ps.InvoiceId)
                .IsRequired();

            //builder.HasOne(ps => ps.Invoice)
            //    .WithOne(i => i.ProductSale) // Assumes a one-to-one relationship with `Invoice`
            //    .HasForeignKey<ProductSale>(ps => ps.InvoiceId)
            //    .OnDelete(DeleteBehavior.Cascade); // Deletes ProductSale when Invoice is deleted

            builder.HasMany(ps => ps.ProductSaleItems)
                .WithOne(psi => psi.ProductSale)
                .HasForeignKey(psi => psi.ProductSaleId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
 