using MediatR;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.CreateScannerEvent;
using TicketAPI.Domain.Storage.GetScannerById;

namespace TicketAPI.Domain.UseCases.CreateScannerEvent
{
    public class CreateScannerEventRequestHandler : IRequestHandler<CreateScannerEventRequest, Guid>
    {
        private readonly IGetScannerByIdStorage _getStorage;
        private readonly ICreateScannerEventStorage _createStorage;

        public CreateScannerEventRequestHandler(IGetScannerByIdStorage getStorage, ICreateScannerEventStorage createStorage)
        {
            _getStorage = getStorage;
            _createStorage = createStorage;
        }

        public async Task<Guid> Handle(CreateScannerEventRequest request, CancellationToken cancellationToken)
        {
            var exists = await _getStorage.IsExistsAsync(request.ScannerId, cancellationToken);
            if (!exists) throw new ScannerNotFoundException(request.ScannerId);

            return await _createStorage.CreateAsync(request.ScannerId, request.EventId, cancellationToken);
        }
    }
}
