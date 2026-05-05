using MediatR;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.Storage.CreateTicket;

namespace TicketAPI.Domain.UseCases.CreateTicket
{
    public class CreateTicketRequestHandler : IRequestHandler<CreateTicketRequest, Guid>
    {
        private readonly ICreateTicketStorage _storage;

        public CreateTicketRequestHandler(ICreateTicketStorage storage)
        {
            _storage = storage;
        }

        public async Task<Guid> Handle(CreateTicketRequest request, CancellationToken cancellationToken)
        {
            var qrCode = Guid.NewGuid().ToString("N");
            return await _storage.CreateAsync(
                request.OrderItemId,
                request.EventId,
                request.SectionId,
                request.SeatId,
                qrCode,
                ETicketStatus.Unused,
                cancellationToken);
        }
    }
}
