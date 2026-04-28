using TicketAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketAPI.DAL.Configurations
{
    public class TicketValidationConfiguration : IEntityTypeConfiguration<TicketValidation>
    {
        public void Configure(EntityTypeBuilder<TicketValidation> entity)
        {
            entity.HasKey(tv => tv.Id);

            entity.HasIndex(tv => tv.TicketId);
            entity.HasIndex(tv => tv.ScannerId);
            entity.HasIndex(tv => tv.ScannedAt);

            entity.Property(tv => tv.ValidatedBy).IsRequired().HasMaxLength(100);

            entity.HasOne(tv => tv.Ticket)
                  .WithMany(t => t.TicketValidations)
                  .HasForeignKey(tv => tv.TicketId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tv => tv.Scanner)
                  .WithMany(s => s.TicketValidations)
                  .HasForeignKey(tv => tv.ScannerId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
