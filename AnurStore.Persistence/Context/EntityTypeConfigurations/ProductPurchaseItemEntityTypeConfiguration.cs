using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class ProductPurchaseItemEntityTypeConfiguration : IEntityTypeConfiguration<ProductPurchaseItem>
    {
        public void Configure(EntityTypeBuilder<ProductPurchaseItem> builder)
        {
            builder.ToTable("ProductPurchaseItems");

            builder.HasKey(ppi => ppi.Id);

            builder.Property(ppi => ppi.ProductId)
                .IsRequired();

            builder.Property(ppi => ppi.Rate)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ppi => ppi.Quantity)
                .IsRequired();

            builder.Property(ppi => ppi.TotalCost)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ppi => ppi.ProductPurchaseId)
                .IsRequired();

            builder.HasOne(ppi => ppi.ProductPurchase)
                .WithMany(pp => pp.PurchaseItems)
                .HasForeignKey(ppi => ppi.ProductPurchaseId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
