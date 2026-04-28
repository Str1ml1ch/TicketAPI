using System.Linq.Expressions;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.Tickets
{
    public sealed class TicketByOrderItemIdSpecification : ISpecification<Ticket>
    {
        private readonly Guid _orderItemId;
        public TicketByOrderItemIdSpecification(Guid orderItemId) => _orderItemId = orderItemId;
        public Expression<Func<Ticket, bool>> ToExpression() => t => t.OrderItemId == _orderItemId;
    }
}
