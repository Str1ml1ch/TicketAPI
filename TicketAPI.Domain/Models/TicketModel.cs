using TicketAPI.Domain.Enums;

namespace TicketAPI.Domain.Models
{
    public class TicketModel
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public Guid EventId { get; set; }
        public Guid SectionId { get; set; }
        public Guid? SeatId { get; set; }
        public string QRCode { get; set; } = null!;
        public ETicketStatus Status { get; set; }
        public DateTimeOffset? UsedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
