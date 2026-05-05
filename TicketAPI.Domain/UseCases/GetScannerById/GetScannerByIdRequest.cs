using MediatR;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.UseCases.GetScannerById
{
    public class GetScannerByIdRequest : IRequest<ScannerModel>
    {
        public Guid ScannerId { get; set; }
    }
}
