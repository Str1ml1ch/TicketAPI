using MediatR;

namespace TicketAPI.Domain.UseCases.RemoveScanner
{
    public class RemoveScannerRequest : IRequest<bool>
    {
        public Guid ScannerId { get; set; }
    }
}
