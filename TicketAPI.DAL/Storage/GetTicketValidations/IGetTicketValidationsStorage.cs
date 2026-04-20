using Homework.Ticketing.System.Shared.Models;
using TicketAPI.Core.Models;

namespace TicketAPI.DAL.Storage.GetTicketValidations
{
    public interface IGetTicketValidationsStorage
    {
        Task<ResultModel<List<TicketValidationModel>>> GetAsync(
            int page,
            int pageSize,
            Guid? ticketId,
            Guid? scannerId,
            CancellationToken ct);
    }
}
