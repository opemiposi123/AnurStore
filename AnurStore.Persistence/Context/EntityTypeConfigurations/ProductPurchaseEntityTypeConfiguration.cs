using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class ProductPurchaseEntityTypeConfiguration : IEntityTypeConfiguration<ProductPurchase>
    {
        public void Configure(EntityTypeBuilder<ProductPurchase> builder)
        {
            builder.ToTable("ProductPurchases");

            builder.HasKey(pp => pp.Id);

            builder.Property(pp => pp.Total)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(pp => pp.Discount)
                .HasColumnType("money");
 
            builder.Property(pp => pp.SupplierId)
                .IsRequired();

            builder.Property(pp => pp.PurchaseDate)
                .IsRequired();

            builder.Property(pp => pp.IsAddedToInventory)
                .IsRequired();

           
            builder.HasMany(pp => pp.PurchaseItems)
                .WithOne(pi => pi.ProductPurchase)
                .HasForeignKey(pi => pi.ProductPurchaseId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
