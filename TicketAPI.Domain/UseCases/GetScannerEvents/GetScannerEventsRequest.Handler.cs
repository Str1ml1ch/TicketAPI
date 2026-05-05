using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Models;
using TicketAPI.Domain.Storage.GetScannerEvents;

namespace TicketAPI.Domain.UseCases.GetScannerEvents
{
    public class GetScannerEventsRequestHandler : IRequestHandler<GetScannerEventsRequest, ResultModel<List<ScannerEventModel>>>
    {
        private readonly IGetScannerEventsStorage _storage;

        public GetScannerEventsRequestHandler(IGetScannerEventsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ResultModel<List<ScannerEventModel>>> Handle(GetScannerEventsRequest request, CancellationToken cancellationToken)
        {
            return await _storage.GetAsync(request.Page, request.PageSize, request.ScannerId, request.EventId, cancellationToken);
        }
    }
}
