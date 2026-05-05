using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.Storage.UpdateScanner
{
    public interface IUpdateScannerStorage
    {
        Task UpdateStatusAsync(Guid id, EScannerStatus status, CancellationToken ct);
    }
}
