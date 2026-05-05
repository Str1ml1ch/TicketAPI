using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.UseCases.GetTickets
{
    public class GetTicketsRequest : IRequest<ResultModel<List<TicketModel>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? EventId { get; set; }
        public Guid? OrderItemId { get; set; }
        public Guid? SeatId { get; set; }
        public ETicketStatus? Status { get; set; }
    }
}
