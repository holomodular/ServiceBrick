using Example.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;
using ServiceBrick.Storage.EntityFrameworkCore;

namespace Example.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        private static void AddCommonServices(IServiceCollection services)
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
        }

        public static IServiceCollection AddBrickExampleEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Database
            var builder = new DbContextOptionsBuilder<ExampleContext>();
            string connectionString = configuration.GetEntityFrameworkCoreConnectionString(
                ExampleEntityFrameworkCoreConstants.APPSETTING_DATABASE_CONNECTION);
            builder.UseSqlServer(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            services.Configure<DbContextOptions<ExampleContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<ExampleContext>>(builder.Options);
            services.AddDbContext<ExampleContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // Add common services
            AddCommonServices(services);

            return services;
        }

        public static IServiceCollection AddBrickExampleEntityFrameworkCoreInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            // In Memory
            var builder = new DbContextOptionsBuilder<ExampleContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString(), b => b.EnableNullChecks(false));
            services.Configure<DbContextOptions<ExampleContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<ExampleContext>>(builder.Options);
            services.AddDbContext<ExampleContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // Add common services
            AddCommonServices(services);

            return services;
        }
    }
}