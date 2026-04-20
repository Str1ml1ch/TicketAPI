using Shared.DAL.Entities;
using TicketAPI.Core.Enums;

namespace TicketAPI.DAL.Entities
{
    public class Ticket : BaseDbEntity
    {
        public Guid OrderItemId { get; set; }
        public Guid EventId { get; set; }
        public Guid SectionId { get; set; }
        public Guid? SeatId { get; set; }
        public string QRCode { get; set; } = null!;
        public ETicketStatus Status { get; set; }
        public DateTimeOffset? UsedAt { get; set; }

        public virtual ICollection<TicketValidation> TicketValidations { get; set; } = new List<TicketValidation>();

    }
}
