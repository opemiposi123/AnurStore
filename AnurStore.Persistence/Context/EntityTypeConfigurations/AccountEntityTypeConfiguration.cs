using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(25); 

            builder.Property(a => a.Description)
                .HasMaxLength(100); 

            builder.Property(a => a.Balance)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(a => a.AccountType)
                .IsRequired(); 
        }
    }
}
