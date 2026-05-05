using MediatR;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.CreateTicketValidation;
using TicketAPI.Domain.Storage.GetTicketById;
using TicketAPI.Domain.Storage.UpdateTicket;

namespace TicketAPI.Domain.UseCases.ValidateTicket
{
    public class ValidateTicketRequestHandler : IRequestHandler<ValidateTicketRequest, Guid>
    {
        private readonly IGetTicketByIdStorage _getStorage;
        private readonly IUpdateTicketStorage _updateStorage;
        private readonly ICreateTicketValidationStorage _validationStorage;

        public ValidateTicketRequestHandler(
            IGetTicketByIdStorage getStorage,
            IUpdateTicketStorage updateStorage,
            ICreateTicketValidationStorage validationStorage)
        {
            _getStorage = getStorage;
            _updateStorage = updateStorage;
            _validationStorage = validationStorage;
        }

        public async Task<Guid> Handle(ValidateTicketRequest request, CancellationToken cancellationToken)
        {
            var ticket = await _getStorage.GetByIdAsync(request.TicketId, cancellationToken);
            if (ticket is null) throw new TicketNotFoundException(request.TicketId);

            await _updateStorage.UpdateStatusAsync(request.TicketId, ETicketStatus.Used, DateTimeOffset.UtcNow, cancellationToken);

            return await _validationStorage.CreateAsync(
                request.TicketId,
                request.ScannerId,
                request.ScannedBy,
                request.ValidatedBy,
                DateTimeOffset.UtcNow,
                cancellationToken);
        }
    }
}
