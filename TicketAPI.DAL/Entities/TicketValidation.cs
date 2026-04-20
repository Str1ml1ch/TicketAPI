using Shared.DAL.Entities;

namespace TicketAPI.DAL.Entities
{
    public class TicketValidation : BaseDbEntity
    {
        public Guid TicketId { get; set; }
        public DateTimeOffset ValidationTime { get; set; }
        public string ValidatedBy { get; set; } = null!;
        public Guid ScannedBy { get; set; }
        public Guid? ScannerId { get; set; }
        public DateTimeOffset ScannedAt { get; set; }

        public virtual Scanner? Scanner { get; set; }
        public virtual Ticket Ticket { get; set; } = null!;
    }
}
