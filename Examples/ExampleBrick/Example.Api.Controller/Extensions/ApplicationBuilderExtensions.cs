using Microsoft.AspNetCore.Builder;

namespace Example.Api.Controller
{
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder RegisterBrickExampleApiController(this IApplicationBuilder applicationBuilder)
        {
            // Register Business rules
            return applicationBuilder;
        }

        public static IApplicationBuilder StartBrickExampleApiController(this IApplicationBuilder applicationBuilder)
        {
            // Start Brick
            BrickStarted = true;
            return applicationBuilder;
        }
    }
}