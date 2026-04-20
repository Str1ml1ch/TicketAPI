using TicketAPI.Core.Models;

namespace TicketAPI.DAL.Storage.GetTicketById
{
    public interface IGetTicketByIdStorage
    {
        Task<TicketDetailModel?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<TicketModel?> GetByQrCodeAsync(string qrCode, CancellationToken ct);
        Task<bool> IsExistsAsync(Guid id, CancellationToken ct);
    }
}
