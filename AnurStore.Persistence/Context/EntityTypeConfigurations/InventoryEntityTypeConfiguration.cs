using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class InventoryEntityTypeConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductId)
                   .IsRequired();

            builder.Property(i => i.BatchNumber)
                   .IsRequired()
                   .HasMaxLength(50); 

            builder.Property(i => i.Remark)
                   .HasMaxLength(250); 

            builder.Property(i => i.StockStatus)
                   .IsRequired();

            builder.HasOne(i => i.Product)
                   .WithOne(up => up.Inventory)
                   .HasForeignKey<Inventory>(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(i => i.StockDate)
                   .HasColumnType("datetime");

            builder.Property(i => i.ExpirationDate)
                   .HasColumnType("datetime");
        }
    }
}
