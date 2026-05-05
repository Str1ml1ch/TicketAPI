namespace TicketAPI.Domain.Storage.RemoveScannerEvent
{
    public interface IRemoveScannerEventStorage
    {
        Task RemoveByIdAsync(Guid id, CancellationToken ct);
        Task RemoveAllByScannerIdAsync(Guid scannerId, CancellationToken ct);
    }
}
