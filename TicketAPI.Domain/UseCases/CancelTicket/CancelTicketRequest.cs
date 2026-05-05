using MediatR;

namespace TicketAPI.Domain.UseCases.CancelTicket
{
    public class CancelTicketRequest : IRequest<bool>
    {
        public Guid TicketId { get; set; }
    }
}
