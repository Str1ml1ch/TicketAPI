using MediatR;
using TicketAPI.Domain.Storage.CreateScanner;

namespace TicketAPI.Domain.UseCases.CreateScanner
{
    public class CreateScannerRequestHandler : IRequestHandler<CreateScannerRequest, Guid>
    {
        private readonly ICreateScannerStorage _storage;

        public CreateScannerRequestHandler(ICreateScannerStorage storage)
        {
            _storage = storage;
        }

        public async Task<Guid> Handle(CreateScannerRequest request, CancellationToken cancellationToken)
        {
            return await _storage.CreateAsync(request.SerialNumber, request.Status, cancellationToken);
        }
    }
}
