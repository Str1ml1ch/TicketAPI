using MediatR;
using TicketAPI.Domain.Models;
using TicketAPI.Domain.Exceptions;
using TicketAPI.Domain.Storage.GetTicketById;

namespace TicketAPI.Domain.UseCases.GetTicketById
{
    public class GetTicketByIdRequestHandler : IRequestHandler<GetTicketByIdRequest, TicketDetailModel>
    {
        private readonly IGetTicketByIdStorage _storage;

        public GetTicketByIdRequestHandler(IGetTicketByIdStorage storage)
        {
            _storage = storage;
        }

        public async Task<TicketDetailModel> Handle(GetTicketByIdRequest request, CancellationToken cancellationToken)
        {
            var ticket = await _storage.GetByIdAsync(request.TicketId, cancellationToken);
            if (ticket is null) throw new TicketNotFoundException(request.TicketId);
            return ticket;
        }
    }
}
