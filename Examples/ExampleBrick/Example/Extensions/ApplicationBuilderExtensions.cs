using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;

namespace Example
{
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder RegisterBrickExample(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Register Business rules
                var registry = serviceScope.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();

                // Validation business rules
                ProductRule.RegisterRule(registry);
                CategoryRule.RegisterRule(registry);
            }
            return applicationBuilder;
        }

        public static IApplicationBuilder StartBrickExample(this IApplicationBuilder applicationBuilder)
        {
            // Start Brick
            BrickStarted = true;
            return applicationBuilder;
        }
    }
}