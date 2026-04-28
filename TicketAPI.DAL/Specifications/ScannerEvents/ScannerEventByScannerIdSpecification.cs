using System.Linq.Expressions;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.ScannerEvents
{
    public sealed class ScannerEventByScannerIdSpecification : ISpecification<ScannerEvent>
    {
        private readonly Guid _scannerId;
        public ScannerEventByScannerIdSpecification(Guid scannerId) => _scannerId = scannerId;
        public Expression<Func<ScannerEvent, bool>> ToExpression() => se => se.ScannerId == _scannerId;
    }
}
