using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrickExample(this IServiceCollection services, IConfiguration configuration)
        {
            // Background Tasks
            services.AddHostedService<ExampleExpireProductTimer>();
            services.AddScoped<ExampleExpireProductTask.Worker>();

            return services;
        }
    }
}