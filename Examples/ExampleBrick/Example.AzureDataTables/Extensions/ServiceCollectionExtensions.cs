using Example.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;

namespace Example.AzureDataTables
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrickExampleAzureDataTables(this IServiceCollection services, IConfiguration configuration)
        {
            // Storage Services
            services.AddScoped<IStorageRepository<Product>, ExampleStorageRepository<Product>>();
            services.AddScoped<IStorageRepository<Category>, ExampleStorageRepository<Category>>();
            services.AddScoped<IStorageRepository<ProductCategory>, ExampleStorageRepository<ProductCategory>>();

            // API Services
            services.AddScoped<IApiService<ProductDto>, ProductApiService>();
            services.AddScoped<IProductApiService, ProductApiService>();

            services.AddScoped<IApiService<CategoryDto>, CategoryApiService>();
            services.AddScoped<ICategoryApiService, CategoryApiService>();

            return services;
        }
    }
}