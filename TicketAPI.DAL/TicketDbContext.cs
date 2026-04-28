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

        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketDbContext).Assembly);
        }
    }
}
