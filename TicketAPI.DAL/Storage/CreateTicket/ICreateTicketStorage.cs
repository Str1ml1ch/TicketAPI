using TicketAPI.Core.Enums;

namespace TicketAPI.DAL.Storage.CreateTicket
{
    public interface ICreateTicketStorage
    {
        Task<Guid> CreateAsync(Guid orderItemId, Guid eventId, Guid sectionId, Guid? seatId, string qrCode, ETicketStatus status, CancellationToken ct);
    }
}
