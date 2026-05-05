using TicketAPI.Domain.Storage.GetTicketById;
using Microsoft.EntityFrameworkCore;
using TicketAPI.Domain.Models;

namespace TicketAPI.DAL.Storage.GetTicketById
{
    public class GetTicketByIdStorage : IGetTicketByIdStorage
    {
        private readonly TicketDbContext _context;

        public GetTicketByIdStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<TicketDetailModel?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Tickets
                .Where(t => t.Id == id)
                .Select(t => new TicketDetailModel
                {
                    Id = t.Id,
                    OrderItemId = t.OrderItemId,
                    EventId = t.EventId,
                    SectionId = t.SectionId,
                    SeatId = t.SeatId,
                    QRCode = t.QRCode,
                    Status = t.Status,
                    UsedAt = t.UsedAt,
                    Validations = t.TicketValidations.Select(tv => new TicketValidationModel
                    {
                        Id = tv.Id,
                        TicketId = tv.TicketId,
                        ValidationTime = tv.ValidationTime,
                        ValidatedBy = tv.ValidatedBy,
                        ScannedBy = tv.ScannedBy,
                        ScannerId = tv.ScannerId,
                        ScannerSerialNumber = tv.Scanner != null ? tv.Scanner.SerialNumber : null,
                        CreatedAt = tv.CreatedAt
                    }).ToList(),
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<TicketModel?> GetByQrCodeAsync(string qrCode, CancellationToken ct)
        {
            return await _context.Tickets
                .Where(t => t.QRCode == qrCode)
                .Select(t => new TicketModel
                {
                    Id = t.Id,
                    OrderItemId = t.OrderItemId,
                    EventId = t.EventId,
                    SectionId = t.SectionId,
                    SeatId = t.SeatId,
                    QRCode = t.QRCode,
                    Status = t.Status,
                    UsedAt = t.UsedAt,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> IsExistsAsync(Guid id, CancellationToken ct)
        {
            return await _context.Tickets.AnyAsync(t => t.Id == id, ct);
        }
    }
}
