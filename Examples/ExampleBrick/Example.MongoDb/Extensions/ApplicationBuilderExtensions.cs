using Example.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;

namespace Example.MongoDb
{
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder RegisterBrickExampleMongoDb(this IApplicationBuilder applicationBuilder)
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

                // This rule populates categories for a product
                ProductGetItemRule.RegisterRule(registry);
            }
            return applicationBuilder;
        }

        public static IApplicationBuilder StartBrickExampleMongoDb(this IApplicationBuilder applicationBuilder)
        {
            BrickStarted = true;
            return applicationBuilder;
        }
    }
}