using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.UseCases.GetScannerEvents
{
    public class GetScannerEventsRequest : IRequest<ResultModel<List<ScannerEventModel>>>
    {
        public Guid ScannerId { get; set; }
        public Guid? EventId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
