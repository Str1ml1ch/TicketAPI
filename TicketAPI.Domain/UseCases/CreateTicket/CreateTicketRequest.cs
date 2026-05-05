using MediatR;
using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.UseCases.CreateTicket
{
    public class CreateTicketRequest : IRequest<Guid>
    {
        public Guid OrderItemId { get; set; }
        public Guid EventId { get; set; }
        public Guid SectionId { get; set; }
        public Guid? SeatId { get; set; }
    }
}
