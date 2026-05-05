using MediatR;

namespace TicketAPI.Domain.UseCases.CreateScannerEvent
{
    public class CreateScannerEventRequest : IRequest<Guid>
    {
        public Guid ScannerId { get; set; }
        public Guid EventId { get; set; }
    }
}
