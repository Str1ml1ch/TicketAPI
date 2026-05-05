using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketAPI.DAL.Storage.CreateScanner;
using TicketAPI.Domain.Storage.CreateScanner;
using TicketAPI.DAL.Storage.CreateScannerEvent;
using TicketAPI.Domain.Storage.CreateScannerEvent;
using TicketAPI.DAL.Storage.CreateTicket;
using TicketAPI.Domain.Storage.CreateTicket;
using TicketAPI.DAL.Storage.CreateTicketValidation;
using TicketAPI.Domain.Storage.CreateTicketValidation;
using TicketAPI.DAL.Storage.GetScannerById;
using TicketAPI.Domain.Storage.GetScannerById;
using TicketAPI.DAL.Storage.GetScannerEvents;
using TicketAPI.Domain.Storage.GetScannerEvents;
using TicketAPI.DAL.Storage.GetScanners;
using TicketAPI.Domain.Storage.GetScanners;
using TicketAPI.DAL.Storage.GetTicketById;
using TicketAPI.Domain.Storage.GetTicketById;
using TicketAPI.DAL.Storage.GetTickets;
using TicketAPI.Domain.Storage.GetTickets;
using TicketAPI.DAL.Storage.GetTicketValidations;
using TicketAPI.Domain.Storage.GetTicketValidations;
using TicketAPI.DAL.Storage.RemoveScanner;
using TicketAPI.Domain.Storage.RemoveScanner;
using TicketAPI.DAL.Storage.RemoveScannerEvent;
using TicketAPI.Domain.Storage.RemoveScannerEvent;
using TicketAPI.DAL.Storage.RemoveTicket;
using TicketAPI.Domain.Storage.RemoveTicket;
using TicketAPI.DAL.Storage.UpdateScanner;
using TicketAPI.Domain.Storage.UpdateScanner;
using TicketAPI.DAL.Storage.UpdateTicket;
using TicketAPI.Domain.Storage.UpdateTicket;

namespace TicketAPI.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContextPool<TicketDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ICreateTicketStorage, CreateTicketStorage>()
                .AddScoped<ICreateTicketValidationStorage, CreateTicketValidationStorage>()
                .AddScoped<ICreateScannerStorage, CreateScannerStorage>()
                .AddScoped<ICreateScannerEventStorage, CreateScannerEventStorage>()
                .AddScoped<IGetTicketsStorage, GetTicketsStorage>()
                .AddScoped<IGetTicketByIdStorage, GetTicketByIdStorage>()
                .AddScoped<IGetTicketValidationsStorage, GetTicketValidationsStorage>()
                .AddScoped<IGetScannersStorage, GetScannersStorage>()
                .AddScoped<IGetScannerByIdStorage, GetScannerByIdStorage>()
                .AddScoped<IGetScannerEventsStorage, GetScannerEventsStorage>()
                .AddScoped<IRemoveTicketStorage, RemoveTicketStorage>()
                .AddScoped<IRemoveScannerStorage, RemoveScannerStorage>()
                .AddScoped<IRemoveScannerEventStorage, RemoveScannerEventStorage>()
                .AddScoped<IUpdateTicketStorage, UpdateTicketStorage>()
                .AddScoped<IUpdateScannerStorage, UpdateScannerStorage>();
        }
    }
}
