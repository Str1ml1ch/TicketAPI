namespace TicketAPI.Domain.Storage.RemoveTicket
{
    public interface IRemoveTicketStorage
    {
        Task RemoveByIdAsync(Guid id, CancellationToken ct);
    }
}
