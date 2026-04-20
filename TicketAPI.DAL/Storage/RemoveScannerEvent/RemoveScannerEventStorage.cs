using Microsoft.EntityFrameworkCore;

namespace TicketAPI.DAL.Storage.RemoveScannerEvent
{
    public class RemoveScannerEventStorage : IRemoveScannerEventStorage
    {
        private readonly TicketDbContext _context;

        public RemoveScannerEventStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task RemoveByIdAsync(Guid id, CancellationToken ct)
        {
            var scannerEvent = await _context.ScannerEvents.FirstOrDefaultAsync(se => se.Id == id, ct);

            _context.ScannerEvents.Remove(scannerEvent!);
            await _context.SaveChangesAsync(ct);
        }

        public async Task RemoveAllByScannerIdAsync(Guid scannerId, CancellationToken ct)
        {
            var events = await _context.ScannerEvents
                .Where(se => se.ScannerId == scannerId)
                .ToListAsync(ct);

            _context.ScannerEvents.RemoveRange(events);
            await _context.SaveChangesAsync(ct);
        }
    }
}
