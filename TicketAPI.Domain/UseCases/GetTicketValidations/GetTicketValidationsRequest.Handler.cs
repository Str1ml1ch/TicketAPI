using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Models;
using TicketAPI.Domain.Storage.GetTicketValidations;

namespace TicketAPI.Domain.UseCases.GetTicketValidations
{
    public class GetTicketValidationsRequestHandler : IRequestHandler<GetTicketValidationsRequest, ResultModel<List<TicketValidationModel>>>
    {
        private readonly IGetTicketValidationsStorage _storage;

        public GetTicketValidationsRequestHandler(IGetTicketValidationsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ResultModel<List<TicketValidationModel>>> Handle(GetTicketValidationsRequest request, CancellationToken cancellationToken)
        {
            return await _storage.GetAsync(request.Page, request.PageSize, request.TicketId, request.ScannerId, cancellationToken);
        }
    }
}
