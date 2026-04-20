using TicketAPI.Core.Enums;

namespace TicketAPI.DAL.Storage.UpdateTicket
{
    public interface IUpdateTicketStorage
    {
        Task UpdateStatusAsync(Guid id, ETicketStatus status, DateTimeOffset? usedAt, CancellationToken ct);
    }
}
