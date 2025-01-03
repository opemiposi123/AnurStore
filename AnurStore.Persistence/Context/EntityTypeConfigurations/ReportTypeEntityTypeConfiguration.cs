using AnurStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnurStore.Persistence.Context.EntityTypeConfigurations
{
    internal sealed class ReportTypeEntityTypeConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(200); 

            builder.Property(r => r.GeneratedDate)
                .IsRequired();

            builder.Property(r => r.ReportType)
                .IsRequired(); 

            builder.Property(r => r.GeneratedById)
                .IsRequired();

            builder.Property(r => r.FilePath)
                .IsRequired()
                .HasMaxLength(500); 

            builder.Property(r => r.ReportData)
                .IsRequired();

            //// Relationships
            //builder.HasOne(r => r.GeneratedBy)
            //    .WithMany(u => u.Reports)
            //    .HasForeignKey(r => r.GeneratedById)
            //    .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete for users
        }
    }
}
