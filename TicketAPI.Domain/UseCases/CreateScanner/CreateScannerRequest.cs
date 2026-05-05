using MediatR;
using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.UseCases.CreateScanner
{
    public class CreateScannerRequest : IRequest<Guid>
    {
        public string SerialNumber { get; set; } = null!;
        public EScannerStatus Status { get; set; } = EScannerStatus.Active;
    }
}
