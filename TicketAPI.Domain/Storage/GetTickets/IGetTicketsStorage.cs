using Homework.Ticketing.System.Shared.Models;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.Storage.GetTickets
{
    public interface IGetTicketsStorage
    {
        Task<ResultModel<List<TicketModel>>> GetAsync(
            int page,
            int pageSize,
            Guid? eventId,
            Guid? orderItemId,
            Guid? seatId,
            ETicketStatus? status,
            CancellationToken ct);
    }
}
