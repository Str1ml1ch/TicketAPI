using MediatR;

namespace TicketAPI.Domain.UseCases.RemoveScannerEvent
{
    public class RemoveScannerEventRequest : IRequest<bool>
    {
        public Guid ScannerEventId { get; set; }
    }
}
