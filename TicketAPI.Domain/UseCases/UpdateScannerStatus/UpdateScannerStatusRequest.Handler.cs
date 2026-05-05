using MediatR;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.GetScannerById;
using TicketAPI.Domain.Storage.UpdateScanner;

namespace TicketAPI.Domain.UseCases.UpdateScannerStatus
{
    public class UpdateScannerStatusRequestHandler : IRequestHandler<UpdateScannerStatusRequest, bool>
    {
        private readonly IGetScannerByIdStorage _getStorage;
        private readonly IUpdateScannerStorage _updateStorage;

        public UpdateScannerStatusRequestHandler(IGetScannerByIdStorage getStorage, IUpdateScannerStorage updateStorage)
        {
            _getStorage = getStorage;
            _updateStorage = updateStorage;
        }

        public async Task<bool> Handle(UpdateScannerStatusRequest request, CancellationToken cancellationToken)
        {
            var exists = await _getStorage.IsExistsAsync(request.ScannerId, cancellationToken);
            if (!exists) throw new ScannerNotFoundException(request.ScannerId);

            await _updateStorage.UpdateStatusAsync(request.ScannerId, request.Status, cancellationToken);
            return true;
        }
    }
}
