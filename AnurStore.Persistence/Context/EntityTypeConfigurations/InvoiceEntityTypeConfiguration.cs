using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class InvoiceEntityTypeConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoiceNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(i => i.TotalAmount)
                   .IsRequired()
                   .HasColumnType("money");

            builder.Property(i => i.Discount)
                   .IsRequired()
                   .HasColumnType("money");

            builder.Property(i => i.NetAmount)
                   .IsRequired()
                   .HasColumnType("money");

            builder.Property(i => i.CustomerName)
                   .HasMaxLength(50); 

            builder.Property(i => i.CustomerCare)
                   .HasMaxLength(100); 

            builder.Property(i => i.Notes)
                   .HasMaxLength(500);

            builder.Property(i => i.PaymentMethod)
                   .IsRequired();

            builder.HasMany(i => i.InvoiceItems)
                   .WithOne(ii => ii.Invoice)
                   .HasForeignKey(ii => ii.InvoiceId)
                   .OnDelete(DeleteBehavior.NoAction); 
        }
    }


}
