using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations;

internal sealed class ProductSizeEntityTypeConfiguration : IEntityTypeConfiguration<ProductSize>
{
    public void Configure(EntityTypeBuilder<ProductSize> builder)
    {
        builder.ToTable("ProductSizes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Size);

        builder.HasOne(ps => ps.Product)
            .WithOne(p => p.ProductSize)
            .HasForeignKey<ProductSize>(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.ProductUnit)
            .WithMany()
            .HasForeignKey(ps => ps.ProductUnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
