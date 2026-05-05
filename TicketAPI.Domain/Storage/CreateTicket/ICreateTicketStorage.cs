using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.Storage.CreateTicket
{
    public interface ICreateTicketStorage
    {
        Task<Guid> CreateAsync(Guid orderItemId, Guid eventId, Guid sectionId, Guid? seatId, string qrCode, ETicketStatus status, CancellationToken ct);
    }
}
