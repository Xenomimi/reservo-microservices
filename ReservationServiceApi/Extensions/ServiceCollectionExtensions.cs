using Microsoft.AspNetCore.Cors.Infrastructure;
using ReservationServiceApi;
using ReservationServiceApi.Resolver;
using ReservationServiceApi.Services;

namespace ReservationServiceApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReservationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ReservationDbContext, ReservationDbContext>();
            serviceCollection.AddTransient <ReservationService>();
            serviceCollection.AddScoped<CustomerResolver>();
            serviceCollection.AddScoped<DiscountResolver>();
            return serviceCollection;
        }
    }
}
