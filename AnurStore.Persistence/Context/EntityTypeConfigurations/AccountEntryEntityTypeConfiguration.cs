
using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed  class AccountEntryEntityTypeConfiguration : IEntityTypeConfiguration<AccountEntry>
    {
        public void Configure(EntityTypeBuilder<AccountEntry> builder)
        {
            // Table name
            builder.ToTable("AccountEntries");

            builder.HasKey(ae => ae.Id);

            builder.Property(ae => ae.AccountId)
                .IsRequired();

            builder.Property(ae => ae.Amount)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(ae => ae.EntryType)
                .IsRequired();

            builder.Property(ae => ae.TransactionId)
                .IsRequired();

            //builder.HasOne(ae => ae.Account)
            //    .WithMany(a => a.AccountEntries) // Assumes a `AccountEntries` collection exists in `Account`
            //    .HasForeignKey(ae => ae.AccountId)
            //    .OnDelete(DeleteBehavior.Cascade); // Deletes related AccountEntries when Account is deleted

            builder.HasOne(ae => ae.Transaction)
                .WithMany(t => t.AccountEntries) 
                .HasForeignKey(ae => ae.TransactionId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
