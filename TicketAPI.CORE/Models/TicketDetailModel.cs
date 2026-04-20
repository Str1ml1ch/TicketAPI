using TicketAPI.Core.Enums;

namespace TicketAPI.Core.Models
{
    public class TicketDetailModel
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public Guid EventId { get; set; }
        public Guid SectionId { get; set; }
        public Guid? SeatId { get; set; }
        public string QRCode { get; set; } = null!;
        public ETicketStatus Status { get; set; }
        public DateTimeOffset? UsedAt { get; set; }
        public List<TicketValidationModel> Validations { get; set; } = [];
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
