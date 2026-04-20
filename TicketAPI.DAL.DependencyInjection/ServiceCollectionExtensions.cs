using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketAPI.DAL.Storage.CreateScanner;
using TicketAPI.DAL.Storage.CreateScannerEvent;
using TicketAPI.DAL.Storage.CreateTicket;
using TicketAPI.DAL.Storage.CreateTicketValidation;
using TicketAPI.DAL.Storage.GetScannerById;
using TicketAPI.DAL.Storage.GetScannerEvents;
using TicketAPI.DAL.Storage.GetScanners;
using TicketAPI.DAL.Storage.GetTicketById;
using TicketAPI.DAL.Storage.GetTickets;
using TicketAPI.DAL.Storage.GetTicketValidations;
using TicketAPI.DAL.Storage.RemoveScanner;
using TicketAPI.DAL.Storage.RemoveScannerEvent;
using TicketAPI.DAL.Storage.RemoveTicket;
using TicketAPI.DAL.Storage.UpdateScanner;
using TicketAPI.DAL.Storage.UpdateTicket;

namespace TicketAPI.DAL.DependencyInjection
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
