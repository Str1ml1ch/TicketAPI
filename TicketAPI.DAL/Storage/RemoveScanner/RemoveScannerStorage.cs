using TicketAPI.Domain.Storage.RemoveScanner;
using Microsoft.EntityFrameworkCore;

namespace TicketAPI.DAL.Storage.RemoveScanner
{
    public class RemoveScannerStorage : IRemoveScannerStorage
    {
        private readonly TicketDbContext _context;

        public RemoveScannerStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task RemoveByIdAsync(Guid id, CancellationToken ct)
        {
            var scanner = await _context.Scanners.FirstOrDefaultAsync(s => s.Id == id, ct);

            _context.Scanners.Remove(scanner!);
            await _context.SaveChangesAsync(ct);
        }
    }
}
