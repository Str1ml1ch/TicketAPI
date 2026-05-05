namespace TicketAPI.Domain.Storage.CreateTicketValidation
{
    public interface ICreateTicketValidationStorage
    {
        Task<Guid> CreateAsync(Guid ticketId, Guid? scannerId, Guid scannedBy, string validatedBy, DateTimeOffset validationTime, CancellationToken ct);
    }
}
