namespace TicketAPI.Domain.Exceptions
{
    public class ScannerEventNotFoundException : NotFoundException
    {
        public ScannerEventNotFoundException(Guid id) : base($"Scanner event with id: {id} not found") { }
    }
}
