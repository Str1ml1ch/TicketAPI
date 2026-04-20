using TicketAPI.Core.Models;
using TicketAPI.DAL.Storage.Filters;
using Microsoft.EntityFrameworkCore;
using Homework.Ticketing.System.Shared.Models;

namespace TicketAPI.DAL.Storage.GetScannerEvents
{
    public class GetScannerEventsStorage : IGetScannerEventsStorage
    {
        private readonly TicketDbContext _context;

        public GetScannerEventsStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<List<ScannerEventModel>>> GetAsync(
            int page,
            int pageSize,
            Guid? scannerId,
            Guid? eventId,
            CancellationToken ct)
        {
            var query = _context.ScannerEvents
                .FilterByScannerId(scannerId)
                .FilterByEventId(eventId);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(se => se.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(se => new ScannerEventModel
                {
                    Id = se.Id,
                    ScannerId = se.ScannerId,
                    ScannerSerialNumber = se.Scanner.SerialNumber,
                    EventId = se.EventId,
                    CreatedAt = se.CreatedAt,
                    UpdatedAt = se.UpdatedAt
                })
                .ToListAsync(ct);

            return new ResultModel<List<ScannerEventModel>> { Data = items, Count = totalCount };
        }
    }
}
