using System.Linq.Expressions;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.TicketValidations
{
    public sealed class TicketValidationByTicketIdSpecification : ISpecification<TicketValidation>
    {
        private readonly Guid _ticketId;
        public TicketValidationByTicketIdSpecification(Guid ticketId) => _ticketId = ticketId;
        public Expression<Func<TicketValidation, bool>> ToExpression() => tv => tv.TicketId == _ticketId;
    }
}
