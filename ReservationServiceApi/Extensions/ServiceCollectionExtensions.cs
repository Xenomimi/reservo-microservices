using Microsoft.AspNetCore.Cors.Infrastructure;
using ReservationServiceApi;
using ReservationServiceApi.Services;

namespace CustomerServiceApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReservationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ReservationDbContext, ReservationDbContext>();
            serviceCollection.AddTransient <ReservationService>();
            return serviceCollection;
        }
    }
}
