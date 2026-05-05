namespace TicketAPI.Domain.Storage.RemoveScanner
{
    public interface IRemoveScannerStorage
    {
        Task RemoveByIdAsync(Guid id, CancellationToken ct);
    }
}
