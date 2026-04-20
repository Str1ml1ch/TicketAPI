using TicketAPI.Core.Enums;

namespace TicketAPI.Core.Models
{
    public class ScannerModel
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; } = null!;
        public EScannerStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
