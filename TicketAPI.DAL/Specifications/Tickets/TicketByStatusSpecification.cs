using System.Linq.Expressions;
using TicketAPI.Core.Enums;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.Tickets
{
    public sealed class TicketByStatusSpecification : ISpecification<Ticket>
    {
        private readonly ETicketStatus _status;
        public TicketByStatusSpecification(ETicketStatus status) => _status = status;
        public Expression<Func<Ticket, bool>> ToExpression() => t => t.Status == _status;
    }
}
