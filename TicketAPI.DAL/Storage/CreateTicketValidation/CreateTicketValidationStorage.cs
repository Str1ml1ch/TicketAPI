using TicketAPI.DAL.Entities;

namespace TicketAPI.DAL.Storage.CreateTicketValidation
{
    public class CreateTicketValidationStorage : ICreateTicketValidationStorage
    {
        private readonly TicketDbContext _context;

        public CreateTicketValidationStorage(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Guid ticketId, Guid? scannerId, Guid scannedBy, string validatedBy, DateTimeOffset validationTime, CancellationToken ct)
        {
            var validation = new TicketValidation
            {
                Id = Guid.NewGuid(),
                TicketId = ticketId,
                ScannerId = scannerId,
                ScannedBy = scannedBy,
                ValidatedBy = validatedBy,
                ValidationTime = validationTime,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.TicketValidations.Add(validation);
            await _context.SaveChangesAsync(ct);

            return validation.Id;
        }
    }
}
