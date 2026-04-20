using TicketAPI.Core.Enums;

namespace TicketAPI.DAL.Storage.UpdateScanner
{
    public interface IUpdateScannerStorage
    {
        Task UpdateStatusAsync(Guid id, EScannerStatus status, CancellationToken ct);
    }
}
