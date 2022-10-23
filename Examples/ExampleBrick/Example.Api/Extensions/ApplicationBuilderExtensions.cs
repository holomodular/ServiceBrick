using Microsoft.AspNetCore.Builder;

namespace Example.Api
{
    public static partial class ApplicationBuilderExtensions
    {
        public static bool BrickStarted = false;

        public static IApplicationBuilder RegisterBrickExampleApi(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder;
        }

        public static IApplicationBuilder StartBrickExampleApi(this IApplicationBuilder applicationBuilder)
        {
            BrickStarted = true;
            return applicationBuilder;
        }
    }
}