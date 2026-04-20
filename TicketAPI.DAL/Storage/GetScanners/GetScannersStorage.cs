using Homework.Ticketing.System.Shared.Models;
using Microsoft.EntityFrameworkCore;
using TicketAPI.Core.Enums;
using TicketAPI.Core.Models;
using TicketAPI.DAL.Storage.Filters;

namespace TicketAPI.DAL.Storage.GetScanners
{
    public class GetScannersStorage : IGetScannersStorage
    {
        private readonly TicketDbContext _context;

        public GetScannersStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<List<ScannerModel>>> GetAsync(
            int page,
            int pageSize,
            EScannerStatus? status,
            CancellationToken ct)
        {
            var query = _context.Scanners
                .FilterByStatus(status);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(s => s.SerialNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new ScannerModel
                {
                    Id = s.Id,
                    SerialNumber = s.SerialNumber,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync(ct);

            return new ResultModel<List<ScannerModel>> { Data = items, Count = totalCount };
        }
    }
}
