using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Storage.Filters
{
    public static class ScannerEventQueryExtensions
    {
        public static IQueryable<ScannerEvent> FilterByScannerId(this IQueryable<ScannerEvent> query, Guid? scannerId)
        {
            if (scannerId.HasValue)
                query = query.Where(se => se.ScannerId == scannerId.Value);
            return query;
        }

        public static IQueryable<ScannerEvent> FilterByEventId(this IQueryable<ScannerEvent> query, Guid? eventId)
        {
            if (eventId.HasValue)
                query = query.Where(se => se.EventId == eventId.Value);
            return query;
        }
    }
}
