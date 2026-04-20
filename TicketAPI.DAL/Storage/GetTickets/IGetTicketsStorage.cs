using Homework.Ticketing.System.Shared.Models;
using TicketAPI.Core.Enums;
using TicketAPI.Core.Models;

namespace TicketAPI.DAL.Storage.GetTickets
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
