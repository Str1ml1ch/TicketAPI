namespace TicketAPI.DAL.Storage.RemoveTicket
{
    public interface IRemoveTicketStorage
    {
        Task RemoveByIdAsync(Guid id, CancellationToken ct);
    }
}
