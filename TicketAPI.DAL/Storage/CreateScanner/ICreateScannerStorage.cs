using TicketAPI.Core.Enums;

namespace TicketAPI.DAL.Storage.CreateScanner
{
    public interface ICreateScannerStorage
    {
        Task<Guid> CreateAsync(string serialNumber, EScannerStatus status, CancellationToken ct);
    }
}
