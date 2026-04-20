using Microsoft.EntityFrameworkCore;

namespace TicketAPI.DAL.Storage.RemoveTicket
{
    public class RemoveTicketStorage : IRemoveTicketStorage
    {
        private readonly TicketDbContext _context;

        public RemoveTicketStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task RemoveByIdAsync(Guid id, CancellationToken ct)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);

            _context.Tickets.Remove(ticket!);
            await _context.SaveChangesAsync(ct);
        }
    }
}
