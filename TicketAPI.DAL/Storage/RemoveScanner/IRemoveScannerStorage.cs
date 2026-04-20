namespace TicketAPI.DAL.Storage.RemoveScanner
{
    public interface IRemoveScannerStorage
    {
        Task RemoveByIdAsync(Guid id, CancellationToken ct);
    }
}
