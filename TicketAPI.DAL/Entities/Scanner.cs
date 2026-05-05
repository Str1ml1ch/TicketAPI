using Shared.DAL.Entities;
using TicketAPI.Domain.Enums;

namespace TicketAPI.DAL.Entities
{
    public class Scanner : BaseDbEntity
    {
        public EScannerStatus Status { get; set; }
        public string SerialNumber { get; set; } = null!;

        public virtual ICollection<ScannerEvent> ScannerEvents { get; set; } = new List<ScannerEvent>();
        public virtual ICollection<TicketValidation> TicketValidations { get; set; } = new List<TicketValidation>();
    }
}
