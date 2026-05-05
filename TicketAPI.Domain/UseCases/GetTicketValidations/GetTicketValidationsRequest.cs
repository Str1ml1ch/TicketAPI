using Homework.Ticketing.System.Shared.Models;
using MediatR;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.UseCases.GetTicketValidations
{
    public class GetTicketValidationsRequest : IRequest<ResultModel<List<TicketValidationModel>>>
    {
        public Guid TicketId { get; set; }
        public Guid? ScannerId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
