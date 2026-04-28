using System.Linq.Expressions;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Specifications.TicketValidations
{
    public sealed class TicketValidationByScannerIdSpecification : ISpecification<TicketValidation>
    {
        private readonly Guid _scannerId;
        public TicketValidationByScannerIdSpecification(Guid scannerId) => _scannerId = scannerId;
        public Expression<Func<TicketValidation, bool>> ToExpression() => tv => tv.ScannerId == _scannerId;
    }
}
