using Homework.Ticketing.System.Shared.Models;
using TicketAPI.Core.Enums;
using TicketAPI.Core.Models;

namespace TicketAPI.DAL.Storage.GetScanners
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
