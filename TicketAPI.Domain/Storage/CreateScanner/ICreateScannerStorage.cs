using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.Storage.CreateScanner
{
    public interface ICreateScannerStorage
    {
        Task<Guid> CreateAsync(string serialNumber, EScannerStatus status, CancellationToken ct);
    }
}
