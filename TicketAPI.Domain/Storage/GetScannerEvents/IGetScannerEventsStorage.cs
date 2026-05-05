using Homework.Ticketing.System.Shared.Models;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.Storage.GetScannerEvents
{
    public interface IGetScannerEventsStorage
    {
        Task<ResultModel<List<ScannerEventModel>>> GetAsync(
            int page,
            int pageSize,
            Guid? scannerId,
            Guid? eventId,
            CancellationToken ct);
    }
}
