using TicketAPI.Core.Models;

namespace TicketAPI.DAL.Storage.GetScannerById
{
    public interface IGetScannerByIdStorage
    {
        Task<ScannerModel?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<ScannerModel?> GetBySerialNumberAsync(string serialNumber, CancellationToken ct);
        Task<bool> IsExistsAsync(Guid id, CancellationToken ct);
    }
}
