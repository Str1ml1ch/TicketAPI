using System.Linq.Expressions;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.Tickets
{
    public sealed class TicketByEventIdSpecification : ISpecification<Ticket>
    {
        private readonly Guid _eventId;
        public TicketByEventIdSpecification(Guid eventId) => _eventId = eventId;
        public Expression<Func<Ticket, bool>> ToExpression() => t => t.EventId == _eventId;
    }
}
