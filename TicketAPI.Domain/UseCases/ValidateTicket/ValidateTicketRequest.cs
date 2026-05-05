using MediatR;

namespace TicketAPI.Domain.UseCases.ValidateTicket
{
    public class ValidateTicketRequest : IRequest<Guid>
    {
        public Guid TicketId { get; set; }
        public Guid? ScannerId { get; set; }
        public Guid ScannedBy { get; set; }
        public string ValidatedBy { get; set; } = null!;
    }
}
