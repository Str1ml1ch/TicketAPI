using Microsoft.EntityFrameworkCore;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL
{
    public class TicketDbContext : DbContext
    {
        public DbSet<Scanner> Scanners { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<TicketValidation> TicketValidations { get; set; } = null!;
        public DbSet<ScannerEvent> ScannerEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Scanner>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasIndex(s => s.SerialNumber).IsUnique();
                entity.HasIndex(s => s.Status);

                entity.Property(s => s.SerialNumber)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Status)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(50);

                entity.HasMany(s => s.ScannerEvents)
                      .WithOne(se => se.Scanner)
                      .HasForeignKey(se => se.ScannerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.TicketValidations)
                      .WithOne(tv => tv.Scanner)
                      .HasForeignKey(tv => tv.ScannerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.HasIndex(t => t.QRCode).IsUnique();
                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => t.EventId);
                entity.HasIndex(t => t.OrderItemId);
                entity.HasIndex(t => t.SectionId);

                entity.Property(t => t.OrderItemId)
                      .IsRequired();

                entity.Property(t => t.EventId)
                      .IsRequired();

                entity.Property(t => t.SectionId)
                      .IsRequired();

                entity.Property(t => t.SeatId)
                      .IsRequired(false);

                entity.Property(t => t.QRCode)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(t => t.Status)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(50);

                entity.Property(t => t.UsedAt)
                      .IsRequired(false);

                entity.HasMany(t => t.TicketValidations)
                      .WithOne(tv => tv.Ticket)
                      .HasForeignKey(tv => tv.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TicketValidation>(entity =>
            {
                entity.HasKey(tv => tv.Id);

                entity.HasIndex(tv => tv.TicketId);
                entity.HasIndex(tv => tv.ScannerId);
                entity.HasIndex(tv => tv.ScannedAt);

                entity.Property(tv => tv.ValidatedBy)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasOne(tv => tv.Ticket)
                      .WithMany(t => t.TicketValidations)
                      .HasForeignKey(tv => tv.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tv => tv.Scanner)
                      .WithMany(s => s.TicketValidations)
                      .HasForeignKey(tv => tv.ScannerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ScannerEvent>(entity =>
            {
                entity.HasKey(se => se.Id);

                entity.HasIndex(se => new { se.ScannerId, se.EventId });
                entity.HasIndex(se => se.EventId);

                entity.HasOne(se => se.Scanner)
                      .WithMany(s => s.ScannerEvents)
                      .HasForeignKey(se => se.ScannerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        {
        }
    }
}
