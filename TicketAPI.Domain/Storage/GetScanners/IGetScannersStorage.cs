using Homework.Ticketing.System.Shared.Models;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.Storage.GetScanners
{
    public interface IGetScannersStorage
    {
        Task<ResultModel<List<ScannerModel>>> GetAsync(
            int page,
            int pageSize,
            EScannerStatus? status,
            CancellationToken ct);
    }
}
