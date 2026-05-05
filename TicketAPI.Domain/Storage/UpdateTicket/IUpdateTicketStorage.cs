using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.Storage.UpdateTicket
{
    public interface IUpdateTicketStorage
    {
        Task UpdateStatusAsync(Guid id, ETicketStatus status, DateTimeOffset? usedAt, CancellationToken ct);
    }
}
