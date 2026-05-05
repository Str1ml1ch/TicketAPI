using MediatR;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.RemoveScannerEvent;

namespace TicketAPI.Domain.UseCases.RemoveScannerEvent
{
    public class RemoveScannerEventRequestHandler : IRequestHandler<RemoveScannerEventRequest, bool>
    {
        private readonly IRemoveScannerEventStorage _storage;

        public RemoveScannerEventRequestHandler(IRemoveScannerEventStorage storage)
        {
            _storage = storage;
        }

        public async Task<bool> Handle(RemoveScannerEventRequest request, CancellationToken cancellationToken)
        {
            await _storage.RemoveByIdAsync(request.ScannerEventId, cancellationToken);
            return true;
        }
    }
}
