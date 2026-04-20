using TicketAPI.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace TicketAPI.DAL.Storage.UpdateScanner
{
    public class UpdateScannerStorage : IUpdateScannerStorage
    {
        private readonly TicketDbContext _context;

        public UpdateScannerStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task UpdateStatusAsync(Guid id, EScannerStatus status, CancellationToken ct)
        {
            var scanner = await _context.Scanners.FirstOrDefaultAsync(s => s.Id == id, ct);

            scanner!.Status = status;
            scanner.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync(ct);
        }
    }
}
