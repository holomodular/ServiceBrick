using Azure.Data.Tables;
using Example.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;
using ServiceBrick.Storage.AzureDataTables;

namespace Example.AzureDataTables
{
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder RegisterBrickExampleAzureDataTables(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Register All Business rules
                var registry = serviceScope.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();

                // This rule sets the createdate/updatedate property on create, and updatedate on update
                CreateUpdateDateRule<Product>.RegisterRule(registry);
                CreateUpdateDateRule<Category>.RegisterRule(registry);

                // This rule ensures date is stored as as UTC zero offset
                DateTimeOffsetRule<Product>.RegisterRule(registry, nameof(Product.ExpirationDate));

                // This rule enforces concurrency using the updatedate property
                ApiConcurrencyByUpdateDateRule<Product, ProductDto>.RegisterRule(registry);
                ApiConcurrencyByUpdateDateRule<Category, CategoryDto>.RegisterRule(registry);

                // This rule occurs before an object is created
                ProductCreateRule.RegisterRule(registry);
                CategoryCreateRule.RegisterRule(registry);

                // This rule occurs before a query is processed
                ProductQueryRule.RegisterRule(registry);
                CategoryQueryRule.RegisterRule(registry);

                // This rule populates categories for a product
                ProductGetItemRule.RegisterRule(registry);
            }
            return applicationBuilder;
        }

        public static IApplicationBuilder StartBrickExampleAzureDataTables(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();

                var connectionString = configuration.GetAzureDataTablesConnectionString(
                    ExampleAzureDataTablesConstants.APPSETTINGS_CONNECTION_STRING);

                // Create each table if not exists
                TableClient tableClient = new TableClient(
                    connectionString,
                    ExampleAzureDataTablesConstants.GetTableName(nameof(Product)));
                tableClient.CreateIfNotExists();

                tableClient = new TableClient(
                    connectionString,
                    ExampleAzureDataTablesConstants.GetTableName(nameof(Category)));
                tableClient.CreateIfNotExists();

                tableClient = new TableClient(
                    connectionString,
                    ExampleAzureDataTablesConstants.GetTableName(nameof(ProductCategory)));
                tableClient.CreateIfNotExists();
            }
            BrickStarted = true;
            return applicationBuilder;
        }
    }
}