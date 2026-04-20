using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Storage.Filters
{
    public static class TicketValidationQueryExtensions
    {
        public static IQueryable<TicketValidation> FilterByTicketId(this IQueryable<TicketValidation> query, Guid? ticketId)
        {
            if (ticketId.HasValue)
                query = query.Where(tv => tv.TicketId == ticketId.Value);
            return query;
        }

        public static IQueryable<TicketValidation> FilterByScannerId(this IQueryable<TicketValidation> query, Guid? scannerId)
        {
            if (scannerId.HasValue)
                query = query.Where(tv => tv.ScannerId == scannerId.Value);
            return query;
        }
    }
}
