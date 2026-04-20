using TicketAPI.DAL.Entities;
using TicketAPI.Core.Enums;

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
