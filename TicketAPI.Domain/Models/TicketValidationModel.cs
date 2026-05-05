namespace TicketAPI.Domain.Models
{
    public class TicketValidationModel
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public DateTimeOffset ValidationTime { get; set; }
        public string ValidatedBy { get; set; } = null!;
        public Guid ScannedBy { get; set; }
        public Guid? ScannerId { get; set; }
        public string? ScannerSerialNumber { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
