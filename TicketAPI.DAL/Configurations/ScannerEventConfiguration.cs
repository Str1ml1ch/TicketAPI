using TicketAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketAPI.DAL.Configurations
{
    public class ScannerEventConfiguration : IEntityTypeConfiguration<ScannerEvent>
    {
        public void Configure(EntityTypeBuilder<ScannerEvent> entity)
        {
            entity.HasKey(se => se.Id);

            entity.HasIndex(se => new { se.ScannerId, se.EventId });
            entity.HasIndex(se => se.EventId);

            entity.HasOne(se => se.Scanner)
                  .WithMany(s => s.ScannerEvents)
                  .HasForeignKey(se => se.ScannerId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
