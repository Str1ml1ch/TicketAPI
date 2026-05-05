using TicketAPI.Domain.Storage.CreateTicket;
using TicketAPI.DAL.Entities;
using TicketAPI.Domain.Enums;

namespace TicketAPI.DAL.Storage.CreateTicket
{
    public class CreateTicketStorage : ICreateTicketStorage
    {
        private readonly TicketDbContext _context;

        public CreateTicketStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Guid orderItemId, Guid eventId, Guid sectionId, Guid? seatId, string qrCode, ETicketStatus status, CancellationToken ct)
        {
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                OrderItemId = orderItemId,
                EventId = eventId,
                SectionId = sectionId,
                SeatId = seatId,
                QRCode = qrCode,
                Status = status,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(ct);

            return ticket.Id;
        }
    }
}
