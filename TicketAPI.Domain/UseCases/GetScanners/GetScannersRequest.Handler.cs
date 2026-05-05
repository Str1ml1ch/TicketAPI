using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Models;
using TicketAPI.Domain.Storage.GetScanners;

namespace TicketAPI.Domain.UseCases.GetScanners
{
    public class GetScannersRequestHandler : IRequestHandler<GetScannersRequest, ResultModel<List<ScannerModel>>>
    {
        private readonly IGetScannersStorage _storage;

        public GetScannersRequestHandler(IGetScannersStorage storage)
        {
            _storage = storage;
        }

        public async Task<ResultModel<List<ScannerModel>>> Handle(GetScannersRequest request, CancellationToken cancellationToken)
        {
            return await _storage.GetAsync(request.Page, request.PageSize, request.Status, cancellationToken);
        }
    }
}
