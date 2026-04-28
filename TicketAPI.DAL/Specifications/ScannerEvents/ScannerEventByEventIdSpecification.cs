using System.Linq.Expressions;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.ScannerEvents
{
    public sealed class ScannerEventByEventIdSpecification : ISpecification<ScannerEvent>
    {
        private readonly Guid _eventId;
        public ScannerEventByEventIdSpecification(Guid eventId) => _eventId = eventId;
        public Expression<Func<ScannerEvent, bool>> ToExpression() => se => se.EventId == _eventId;
    }
}
