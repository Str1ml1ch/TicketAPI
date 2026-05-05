using TicketAPI.Domain.Storage.GetScannerById;
using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Models;

namespace TicketAPI.DAL.Storage.GetScannerById
{
    public class GetScannerByIdStorage : IGetScannerByIdStorage
    {
        private readonly TicketDbContext _context;

        public GetScannerByIdStorage(TicketDbContext context)
        {
            _context = context;
        }

        private static ScannerModel ToModel(Entities.Scanner s) => new ScannerModel
        {
            Id = s.Id,
            SerialNumber = s.SerialNumber,
            Status = s.Status,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        public async Task<ScannerModel?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Scanners
                .Where(s => s.Id == id)
                .Select(s => new ScannerModel
                {
                    Id = s.Id,
                    SerialNumber = s.SerialNumber,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ScannerModel?> GetBySerialNumberAsync(string serialNumber, CancellationToken ct)
        {
            return await _context.Scanners
                .Where(s => s.SerialNumber == serialNumber)
                .Select(s => new ScannerModel
                {
                    Id = s.Id,
                    SerialNumber = s.SerialNumber,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> IsExistsAsync(Guid id, CancellationToken ct)
        {
            return await _context.Scanners.AnyAsync(s => s.Id == id, ct);
        }
    }
}
