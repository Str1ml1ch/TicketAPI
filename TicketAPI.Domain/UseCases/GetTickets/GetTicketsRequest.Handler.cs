using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Models;
using TicketAPI.Domain.Storage.GetTickets;

namespace TicketAPI.Domain.UseCases.GetTickets
{
    public class GetTicketsRequestHandler : IRequestHandler<GetTicketsRequest, ResultModel<List<TicketModel>>>
    {
        private readonly IGetTicketsStorage _storage;

        public GetTicketsRequestHandler(IGetTicketsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ResultModel<List<TicketModel>>> Handle(GetTicketsRequest request, CancellationToken cancellationToken)
        {
            return await _storage.GetAsync(
                request.Page,
                request.PageSize,
                request.EventId,
                request.OrderItemId,
                request.SeatId,
                request.Status,
                cancellationToken);
        }
    }
}
