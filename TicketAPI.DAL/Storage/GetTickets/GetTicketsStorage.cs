using Homework.Ticketing.System.Shared.Models;
using Microsoft.EntityFrameworkCore;
using TicketAPI.Core.Enums;
using TicketAPI.Core.Models;
using TicketAPI.DAL.Specifications.Tickets;

namespace TicketAPI.DAL.Storage.GetTickets
{
    public class GetTicketsStorage : IGetTicketsStorage
    {
        private readonly TicketDbContext _context;

        public GetTicketsStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<List<TicketModel>>> GetAsync(
            int page,
            int pageSize,
            Guid? eventId,
            Guid? orderItemId,
            Guid? seatId,
            ETicketStatus? status,
            CancellationToken ct)
        {
            var query = _context.Tickets.AsQueryable();
            if (eventId.HasValue)
                query = query.Where(new TicketByEventIdSpecification(eventId.Value).ToExpression());
            if (orderItemId.HasValue)
                query = query.Where(new TicketByOrderItemIdSpecification(orderItemId.Value).ToExpression());
            if (seatId.HasValue)
                query = query.Where(new TicketBySeatIdSpecification(seatId.Value).ToExpression());
            if (status.HasValue)
                query = query.Where(new TicketByStatusSpecification(status.Value).ToExpression());

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TicketModel
                {
                    Id = t.Id,
                    OrderItemId = t.OrderItemId,
                    EventId = t.EventId,
                    SectionId = t.SectionId,
                    SeatId = t.SeatId,
                    QRCode = t.QRCode,
                    Status = t.Status,
                    UsedAt = t.UsedAt,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync(ct);

            return new ResultModel<List<TicketModel>> { Data = items, Count = totalCount };
        }
    }
}
