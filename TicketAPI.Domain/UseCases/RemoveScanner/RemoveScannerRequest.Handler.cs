using MediatR;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.GetScannerById;
using TicketAPI.Domain.Storage.RemoveScanner;
using TicketAPI.Domain.Storage.RemoveScannerEvent;

namespace TicketAPI.Domain.UseCases.RemoveScanner
{
    public class RemoveScannerRequestHandler : IRequestHandler<RemoveScannerRequest, bool>
    {
        private readonly IGetScannerByIdStorage _getStorage;
        private readonly IRemoveScannerStorage _removeStorage;
        private readonly IRemoveScannerEventStorage _removeEventStorage;

        public RemoveScannerRequestHandler(
            IGetScannerByIdStorage getStorage,
            IRemoveScannerStorage removeStorage,
            IRemoveScannerEventStorage removeEventStorage)
        {
            _getStorage = getStorage;
            _removeStorage = removeStorage;
            _removeEventStorage = removeEventStorage;
        }

        public async Task<bool> Handle(RemoveScannerRequest request, CancellationToken cancellationToken)
        {
            var exists = await _getStorage.IsExistsAsync(request.ScannerId, cancellationToken);
            if (!exists) throw new ScannerNotFoundException(request.ScannerId);

            await _removeEventStorage.RemoveAllByScannerIdAsync(request.ScannerId, cancellationToken);
            await _removeStorage.RemoveByIdAsync(request.ScannerId, cancellationToken);
            return true;
        }
    }
}
