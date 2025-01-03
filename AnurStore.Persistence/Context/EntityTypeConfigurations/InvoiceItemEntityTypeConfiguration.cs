using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class InvoiceItemEntityTypeConfiguration : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            builder.ToTable("InvoiceItems");

            builder.HasKey(ii => ii.Id);

            builder.Property(ii => ii.Quantity)
                .IsRequired();

            builder.Property(ii => ii.Description)
                .HasMaxLength(100);

            builder.Property(ii => ii.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ii => ii.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ii => ii.InvoiceId)
                .IsRequired();

            builder.HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
