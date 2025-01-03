using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations;

internal sealed class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(s => s.Email)
            .IsRequired();
        builder.Property(s => s.Location)
            .IsRequired();

        builder.Property(s => s.PhoneNumber)
            .IsRequired(); 
        builder.HasIndex(s => s.Email).IsUnique(); 
    }
}