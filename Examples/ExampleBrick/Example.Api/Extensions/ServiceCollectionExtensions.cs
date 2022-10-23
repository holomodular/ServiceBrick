using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrickExampleApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoryApiClient, CategoryApiClient>();
            services.AddScoped<IProductApiClient, ProductApiClient>();
            return services;
        }
    }
}