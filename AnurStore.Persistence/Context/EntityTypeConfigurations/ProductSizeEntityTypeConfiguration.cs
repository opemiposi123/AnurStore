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

        builder.HasOne(u => u.Product)
               .WithOne(up => up.ProductSize)
               .HasForeignKey<ProductSize>(x => x.ProductId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
