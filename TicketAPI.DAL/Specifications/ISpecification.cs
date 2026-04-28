using System.Linq.Expressions;

namespace TicketAPI.DAL.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
