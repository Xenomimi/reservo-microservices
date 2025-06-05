using CustomerServiceApi.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace DiscountServiceApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscountServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<DiscountDbContext, DiscountDbContext>();
            serviceCollection.AddTransient <DiscountService>();
            return serviceCollection;
        }
    }
}
