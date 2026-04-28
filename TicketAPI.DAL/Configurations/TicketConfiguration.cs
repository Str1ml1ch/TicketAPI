using TicketAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketAPI.DAL.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> entity)
        {
            entity.HasKey(t => t.Id);

            entity.HasIndex(t => t.QRCode).IsUnique();
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.EventId);
            entity.HasIndex(t => t.OrderItemId);
            entity.HasIndex(t => t.SectionId);

            entity.Property(t => t.OrderItemId).IsRequired();
            entity.Property(t => t.EventId).IsRequired();
            entity.Property(t => t.SectionId).IsRequired();
            entity.Property(t => t.SeatId).IsRequired(false);
            entity.Property(t => t.QRCode).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
            entity.Property(t => t.UsedAt).IsRequired(false);

            entity.HasMany(t => t.TicketValidations)
                  .WithOne(tv => tv.Ticket)
                  .HasForeignKey(tv => tv.TicketId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
