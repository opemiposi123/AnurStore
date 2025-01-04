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
    internal class RecieptItemEntityTypeConfiguration : IEntityTypeConfiguration<RecieptItem>
    {
        public void Configure(EntityTypeBuilder<RecieptItem> builder)
        {
            builder.ToTable("RecieptItems");

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

            builder.Property(ii => ii.RecieptId)
                .IsRequired();

            builder.HasOne(ii => ii.Reciept)
                .WithMany(i => i.RecieptItems)
                .HasForeignKey(ii => ii.RecieptId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
