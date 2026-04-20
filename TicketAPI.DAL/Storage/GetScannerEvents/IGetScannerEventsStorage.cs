using Homework.Ticketing.System.Shared.Models;
using TicketAPI.Core.Models;

namespace TicketAPI.DAL.Storage.GetScannerEvents
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
