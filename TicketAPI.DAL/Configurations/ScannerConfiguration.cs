using TicketAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketAPI.DAL.Configurations
{
    public class ScannerConfiguration : IEntityTypeConfiguration<Scanner>
    {
        public void Configure(EntityTypeBuilder<Scanner> entity)
        {
            entity.HasKey(s => s.Id);

            entity.HasIndex(s => s.SerialNumber).IsUnique();
            entity.HasIndex(s => s.Status);

            entity.Property(s => s.SerialNumber).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Status).IsRequired().HasConversion<string>().HasMaxLength(50);

            entity.HasMany(s => s.ScannerEvents)
                  .WithOne(se => se.Scanner)
                  .HasForeignKey(se => se.ScannerId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(s => s.TicketValidations)
                  .WithOne(tv => tv.Scanner)
                  .HasForeignKey(tv => tv.ScannerId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
