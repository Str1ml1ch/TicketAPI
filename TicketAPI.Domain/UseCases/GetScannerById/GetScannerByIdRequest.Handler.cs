using MediatR;
using TicketAPI.Domain.Models;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.GetScannerById;

namespace TicketAPI.Domain.UseCases.GetScannerById
{
    public class GetScannerByIdRequestHandler : IRequestHandler<GetScannerByIdRequest, ScannerModel>
    {
        private readonly IGetScannerByIdStorage _storage;

        public GetScannerByIdRequestHandler(IGetScannerByIdStorage storage)
        {
            _storage = storage;
        }

        public async Task<ScannerModel> Handle(GetScannerByIdRequest request, CancellationToken cancellationToken)
        {
            var scanner = await _storage.GetByIdAsync(request.ScannerId, cancellationToken);
            if (scanner is null) throw new ScannerNotFoundException(request.ScannerId);
            return scanner;
        }
    }
}
