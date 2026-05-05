namespace TicketAPI.Domain.Exceptions
{
    public class ScannerNotFoundException : NotFoundException
    {
        public ScannerNotFoundException(Guid id) : base($"Scanner with id: {id} not found") { }
    }
}
