using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.UseCases.GetScanners
{
    public class GetScannersRequest : IRequest<ResultModel<List<ScannerModel>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public EScannerStatus? Status { get; set; }
    }
}
