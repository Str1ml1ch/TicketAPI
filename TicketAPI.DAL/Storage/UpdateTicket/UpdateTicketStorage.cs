using TicketAPI.Domain.Storage.UpdateTicket;
using TicketAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace TicketAPI.DAL.Storage.UpdateTicket
{
    public class UpdateTicketStorage : IUpdateTicketStorage
    {
        private readonly TicketDbContext _context;

        public UpdateTicketStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task UpdateStatusAsync(Guid id, ETicketStatus status, DateTimeOffset? usedAt, CancellationToken ct)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);

            ticket!.Status = status;
            ticket.UsedAt = usedAt;
            ticket.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync(ct);
        }
    }
}
