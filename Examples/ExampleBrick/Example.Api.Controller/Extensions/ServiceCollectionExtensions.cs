using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api.Controller
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrickExampleApiController(this IServiceCollection services, IConfiguration configuration)
        {
            // API Controllers
            services.AddScoped<IProductApiController, ProductApiController>();
            services.AddScoped<ICategoryApiController, CategoryApiController>();

            return services;
        }
    }
}