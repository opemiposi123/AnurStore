using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class ProductUnitEntityTypeConfiguration : IEntityTypeConfiguration<ProductUnit>
    {
        public void Configure(EntityTypeBuilder<ProductUnit> builder)
        {
            builder.ToTable("ProductUnits");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(c => c.Description)
                .HasMaxLength(50);
            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}
