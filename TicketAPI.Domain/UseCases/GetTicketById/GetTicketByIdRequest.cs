using MediatR;
using TicketAPI.Domain.Models;

namespace TicketAPI.Domain.UseCases.GetTicketById
{
    public class GetTicketByIdRequest : IRequest<TicketDetailModel>
    {
        public Guid TicketId { get; set; }
    }
}
