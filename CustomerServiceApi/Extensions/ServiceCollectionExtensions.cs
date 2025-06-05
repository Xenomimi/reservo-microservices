using CustomerServiceApi.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CustomerServiceApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CustomerContextDb, CustomerContextDb>();
            serviceCollection.AddTransient <CustomerService>();
            return serviceCollection;
        }
    }
}
