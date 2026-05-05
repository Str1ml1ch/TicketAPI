namespace TicketAPI.Domain.Storage.CreateScannerEvent
{
    public interface ICreateScannerEventStorage
    {
        Task<Guid> CreateAsync(Guid scannerId, Guid eventId, CancellationToken ct);
    }
}
