using MediatR;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.GetTicketById;
using TicketAPI.Domain.Storage.UpdateTicket;

namespace TicketAPI.Domain.UseCases.CancelTicket
{
    public class CancelTicketRequestHandler : IRequestHandler<CancelTicketRequest, bool>
    {
        private readonly IGetTicketByIdStorage _getStorage;
        private readonly IUpdateTicketStorage _updateStorage;

        public CancelTicketRequestHandler(IGetTicketByIdStorage getStorage, IUpdateTicketStorage updateStorage)
        {
            _getStorage = getStorage;
            _updateStorage = updateStorage;
        }

        public async Task<bool> Handle(CancelTicketRequest request, CancellationToken cancellationToken)
        {
            var exists = await _getStorage.IsExistsAsync(request.TicketId, cancellationToken);
            if (!exists) throw new TicketNotFoundException(request.TicketId);

            await _updateStorage.UpdateStatusAsync(request.TicketId, ETicketStatus.Cancelled, null, cancellationToken);
            return true;
        }
    }
}
