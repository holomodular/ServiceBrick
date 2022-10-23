using Example.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;

namespace Example.EntityFrameworkCore
{
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder RegisterBrickExampleEntityFrameworkCore(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Register Business rules
                var registry = serviceScope.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();

                // This rule sets the createdate/updatedate property on create, and updatedate on update
                CreateUpdateDateRule<Product>.RegisterRule(registry);
                CreateUpdateDateRule<Category>.RegisterRule(registry);

                // This rule ensures date is stored as as UTC zero offset
                DateTimeOffsetRule<Product>.RegisterRule(registry, nameof(Product.ExpirationDate));

                // This rule enforces concurrency using the updatedate property
                ApiConcurrencyByUpdateDateRule<Product, ProductDto>.RegisterRule(registry);
                ApiConcurrencyByUpdateDateRule<Category, CategoryDto>.RegisterRule(registry);

                // This rule occurs before a query is processed
                ProductQueryRule.RegisterRule(registry);
                CategoryQueryRule.RegisterRule(registry);
            }
            return applicationBuilder;
        }

        public static IApplicationBuilder RegisterBrickExampleEntityFrameworkCoreInMemory(this IApplicationBuilder applicationBuilder)
        {
            RegisterBrickExampleEntityFrameworkCore(applicationBuilder);
            return applicationBuilder;
        }

        public static IApplicationBuilder StartBrickExampleEntityFrameworkCoreInMemory(this IApplicationBuilder applicationBuilder)
        {
            BrickStarted = true;
            return applicationBuilder;
        }

        public static IApplicationBuilder StartBrickExampleEntityFrameworkCore(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Migrate Example
                var context = serviceScope.ServiceProvider.GetService<ExampleContext>();
                context.Database.Migrate();
                context.SaveChanges();
            }
            BrickStarted = true;
            return applicationBuilder;
        }
    }
}