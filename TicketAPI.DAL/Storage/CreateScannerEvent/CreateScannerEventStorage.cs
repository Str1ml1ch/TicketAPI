using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Storage.CreateScannerEvent
{
    public class CreateScannerEventStorage : ICreateScannerEventStorage
    {
        private readonly TicketDbContext _context;

        public CreateScannerEventStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Guid scannerId, Guid eventId, CancellationToken ct)
        {
            var scannerEvent = new ScannerEvent
            {
                Id = Guid.NewGuid(),
                ScannerId = scannerId,
                EventId = eventId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.ScannerEvents.Add(scannerEvent);
            await _context.SaveChangesAsync(ct);

            return scannerEvent.Id;
        }
    }
}
