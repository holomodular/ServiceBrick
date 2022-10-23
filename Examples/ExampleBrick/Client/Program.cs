using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartDependecyInjection(args);

            ServiceBrickExampleCategory exampleCategory = new ServiceBrickExampleCategory();
            exampleCategory.ShowExample();

            ServiceBrickExampleProduct exampleProduct = new ServiceBrickExampleProduct();
            exampleProduct.ShowExample();
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