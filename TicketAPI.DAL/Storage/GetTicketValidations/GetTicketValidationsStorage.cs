using Homework.Ticketing.System.Shared.Models;
using Microsoft.EntityFrameworkCore;
using TicketAPI.Core.Models;
using TicketAPI.DAL.Storage.Filters;

namespace TicketAPI.DAL.Storage.GetTicketValidations
{
    public class GetTicketValidationsStorage : IGetTicketValidationsStorage
    {
        private readonly TicketDbContext _context;

        public GetTicketValidationsStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<List<TicketValidationModel>>> GetAsync(
            int page,
            int pageSize,
            Guid? ticketId,
            Guid? scannerId,
            CancellationToken ct)
        {
            var query = _context.TicketValidations
                .FilterByTicketId(ticketId)
                .FilterByScannerId(scannerId);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(tv => tv.ValidationTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(tv => new TicketValidationModel
                {
                    Id = tv.Id,
                    TicketId = tv.TicketId,
                    ValidationTime = tv.ValidationTime,
                    ValidatedBy = tv.ValidatedBy,
                    ScannedBy = tv.ScannedBy,
                    ScannerId = tv.ScannerId,
                    ScannerSerialNumber = tv.Scanner != null ? tv.Scanner.SerialNumber : null,
                    CreatedAt = tv.CreatedAt
                })
                .ToListAsync(ct);

            return new ResultModel<List<TicketValidationModel>> { Data = items, Count = totalCount };
        }
    }
}
