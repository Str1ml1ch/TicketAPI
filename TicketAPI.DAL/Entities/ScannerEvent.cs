using Shared.DAL.Entities;

namespace TicketAPI.DAL.Entities
{
    public class ScannerEvent : BaseDbEntity
    {
        public Guid ScannerId { get; set; }
        public Guid EventId { get; set; }

        public virtual Scanner Scanner { get; set; } = null!;
    }
}
