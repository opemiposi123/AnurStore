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
    internal class RecieptEntityTypeConfiguration : IEntityTypeConfiguration<Reciept>
    {
        public void Configure(EntityTypeBuilder<Reciept> builder)
        {
            builder.ToTable("Reciepts");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.RecieptNumber)
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



            builder.Property(i => i.PaymentMethod)
                   .IsRequired();

            builder.HasMany(i => i.RecieptItems)
                   .WithOne(ii => ii.Reciept)
                   .HasForeignKey(ii => ii.RecieptId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
