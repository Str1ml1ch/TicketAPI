namespace TicketAPI.Domain.Exceptions
{
    public class TicketNotFoundException : NotFoundException
    {
        public TicketNotFoundException(Guid id) : base($"Ticket with id: {id} not found") { }
    }
}
