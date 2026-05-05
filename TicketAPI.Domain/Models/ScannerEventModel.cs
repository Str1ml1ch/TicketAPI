namespace TicketAPI.Domain.Models
{
    public class ScannerEventModel
    {
        public Guid Id { get; set; }
        public Guid ScannerId { get; set; }
        public string ScannerSerialNumber { get; set; } = null!;
        public Guid EventId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
