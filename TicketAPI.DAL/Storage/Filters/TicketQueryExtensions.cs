using TicketAPI.DAL.Entities;
using TicketAPI.Core.Enums;

namespace TicketAPI.DAL.Storage.Filters
{
    public static class TicketQueryExtensions
    {
        public static IQueryable<Ticket> FilterByEventId(this IQueryable<Ticket> query, Guid? eventId)
        {
            if (eventId.HasValue)
                query = query.Where(t => t.EventId == eventId.Value);
            return query;
        }

        public static IQueryable<Ticket> FilterByOrderItemId(this IQueryable<Ticket> query, Guid? orderItemId)
        {
            if (orderItemId.HasValue)
                query = query.Where(t => t.OrderItemId == orderItemId.Value);
            return query;
        }

        public static IQueryable<Ticket> FilterBySeatId(this IQueryable<Ticket> query, Guid? seatId)
        {
            if (seatId.HasValue)
                query = query.Where(t => t.SeatId == seatId.Value);
            return query;
        }

        public static IQueryable<Ticket> FilterByStatus(this IQueryable<Ticket> query, ETicketStatus? status)
        {
            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);
            return query;
        }
    }
}
