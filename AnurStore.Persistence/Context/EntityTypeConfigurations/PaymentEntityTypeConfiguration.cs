using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(p => p.PaymentMethod)
                .IsRequired();

            builder.Property(p => p.TransactionReference)
                .IsRequired()
                .HasMaxLength(200); 
            builder.Property(p => p.AccountId)
                .IsRequired();
        }
    }
}
