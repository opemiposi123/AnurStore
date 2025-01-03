using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed  class ProductSaleItemEntityTypeConfiguration : IEntityTypeConfiguration<ProductSaleItem>
    {
        public void Configure(EntityTypeBuilder<ProductSaleItem> builder)
        {
            builder.ToTable("ProductSaleItems");

            builder.HasKey(psi => psi.Id);

            builder.Property(psi => psi.ProductId)
                .IsRequired();

            builder.Property(psi => psi.Quantity)
                .IsRequired();

            builder.Property(psi => psi.ProductUnitType)
                .IsRequired(); 

            builder.Property(psi => psi.SubTotal)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(psi => psi.ProductSaleId)
                .IsRequired();

            //builder.HasOne(psi => psi.Product)
            //    .WithMany(p => p.ProductSaleItems) 
            //    .HasForeignKey(psi => psi.ProductId)
            //    .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(psi => psi.ProductSale)
                .WithMany(ps => ps.ProductSaleItems)
                .HasForeignKey(psi => psi.ProductSaleId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
