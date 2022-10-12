using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using ServiceBrick;
using Microsoft.AspNetCore.Hosting;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartDependecyInjection(args);

            ServiceBrickLoggingExample loggingExample = new ServiceBrickLoggingExample();
            loggingExample.ShowExample();

            ServiceBrickCacheExample cacheExample = new ServiceBrickCacheExample();
            cacheExample.ShowExample();

            ServiceBrickNotificationExample notificationExample = new ServiceBrickNotificationExample();
            notificationExample.ShowExample();

            ServiceBrickSecurityExample securityExample = new ServiceBrickSecurityExample();
            securityExample.ShowExample();
        }

        public static CancellationTokenSource CancellationTokenSource { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static IHost HostObj { get; set; }

        private static void StartDependecyInjection(string[] args)
        {
            // Create host builder
            var hostBuilder = CreateHostBuilder(args);

            // Build
            HostObj = hostBuilder.Build();
            ServiceProvider = HostObj.Services;
            Configuration = StartupClient.Configuration;
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                new StartupClient(hostBuilderContext.Configuration)
                    .ConfigureServices(serviceCollection);
            });
        }
    }
}