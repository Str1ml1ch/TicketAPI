using TicketAPI.DAL.Entities;
using TicketAPI.Core.Enums;

namespace TicketAPI.DAL.Storage.CreateScanner
{
    public class CreateScannerStorage : ICreateScannerStorage
    {
        private readonly TicketDbContext _context;

        public CreateScannerStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(string serialNumber, EScannerStatus status, CancellationToken ct)
        {
            var scanner = new Scanner
            {
                Id = Guid.NewGuid(),
                SerialNumber = serialNumber,
                Status = status,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.Scanners.Add(scanner);
            await _context.SaveChangesAsync(ct);

            return scanner.Id;
        }
    }
}
