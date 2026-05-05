using TicketAPI.Domain.Enums;
using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Storage.Filters
{
    public static class ScannerQueryExtensions
    {
        public static IQueryable<Scanner> FilterByStatus(this IQueryable<Scanner> query, EScannerStatus? status)
        {
            if (status.HasValue)
                query = query.Where(s => s.Status == status.Value);
            return query;
        }
    }
}
