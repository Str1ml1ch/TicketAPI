using System.Linq.Expressions;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.Tickets
{
    public sealed class TicketBySeatIdSpecification : ISpecification<Ticket>
    {
        private readonly Guid _seatId;
        public TicketBySeatIdSpecification(Guid seatId) => _seatId = seatId;
        public Expression<Func<Ticket, bool>> ToExpression() => t => t.SeatId == _seatId;
    }
}
