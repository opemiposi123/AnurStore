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
    internal sealed class ProductPurchaseEntityTypeConfiguration : IEntityTypeConfiguration<ProductPurchase>
    {
        public void Configure(EntityTypeBuilder<ProductPurchase> builder)
        {
            builder.ToTable("ProductPurchases");

            builder.HasKey(pp => pp.Id);

            builder.Property(pp => pp.Total)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(pp => pp.Discount)
                .HasColumnType("money");

            builder.Property(pp => pp.PaymentMethod)
                .IsRequired(); 

            builder.Property(pp => pp.SupplierId)
                .IsRequired();

            builder.Property(pp => pp.PurchaseDate)
                .IsRequired();

            builder.Property(pp => pp.IsAddedToInventory)
                .IsRequired();

           
            builder.HasMany(pp => pp.PurchaseItems)
                .WithOne(pi => pi.ProductPurchase)
                .HasForeignKey(pi => pi.ProductPurchaseId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
