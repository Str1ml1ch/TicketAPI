using MediatR;
using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.UseCases.UpdateScannerStatus
{
    public class UpdateScannerStatusRequest : IRequest<bool>
    {
        public Guid ScannerId { get; set; }
        public EScannerStatus Status { get; set; }
    }
}
