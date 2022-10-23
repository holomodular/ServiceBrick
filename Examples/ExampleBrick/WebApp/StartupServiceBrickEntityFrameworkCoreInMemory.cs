using Example;
using Example.Api;
using Example.Api.Controller;
using Example.EntityFrameworkCore;
using ServiceBrick;
using ServiceBrick.Logging;
using ServiceBrick.Logging.Api;
using ServiceBrick.Logging.Api.Controller;
using ServiceBrick.Logging.EntityFrameworkCore;
using ServiceBrick.Security;
using ServiceBrick.Security.Api;
using ServiceBrick.Security.Api.Controller;
using ServiceBrick.Security.EntityFrameworkCore;
using ServiceBrick.ServiceBus;
using ServiceBrick.ServiceBus.InMemory;
using WebApp.Extensions;

namespace WebApp
{
    /// <summary>
    /// This class starts SERVICE BRICK using the EntityFrameworkCoreInMemory provider.
    /// Change Program.cs to use this class.
    /// </summary>
    public class StartupServiceBrickEntityFrameworkCoreInMemory : ServiceBrick.Startup
    {
        public StartupServiceBrickEntityFrameworkCoreInMemory(IConfiguration configuration) : base(configuration)
        {
        }

        public virtual IWebHostEnvironment WebHostEnvironment { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Call base implementation
            base.CustomConfigureServices(services);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            // Call base implementation
            WebHostEnvironment = webHostEnvironment;
            base.CustomConfigure(app);

            // Startup complete
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupServiceBrickEntityFrameworkCoreInMemory>>();
            logger.LogInformation("Application Started");
        }

        public override void AddBricks(IServiceCollection services)
        {
            // Call base implementation
            base.AddBricks(services);

            // Service Brick Core
            services.AddBrickCore(Configuration, typeof(StartupServiceBrickEntityFrameworkCoreInMemory).Assembly);

            // Service Bus Brick
            services.AddBrickServiceBusInMemory(Configuration);
            services.AddBrickServiceBus(Configuration);

            // Logging Brick
            services.AddBrickLoggingApi(Configuration);
            services.AddBrickLoggingApiController(Configuration);
            services.AddBrickLoggingEntityFrameworkCoreInMemory(Configuration);
            services.AddBrickLogging(Configuration);

            // Security Brick
            services.AddBrickSecurityApi(Configuration);
            services.AddBrickSecurityApiController(Configuration);
            services.AddBrickSecurityEntityFrameworkCoreInMemory(Configuration);
            services.AddBrickSecurity(Configuration);

            // Example Brick
            services.AddBrickExampleApi(Configuration);
            services.AddBrickExampleApiController(Configuration);
            services.AddBrickExampleEntityFrameworkCoreInMemory(Configuration);
            services.AddBrickExample(Configuration);

            // Custom Website
            services.AddCustomWebsite(Configuration);
        }

        public override void RegisterBricks(IApplicationBuilder app)
        {
            // Call base implementation
            base.RegisterBricks(app);

            // Service Brick Core
            app.RegisterBrickCore();

            // Service Bus Brick
            app.RegisterBrickServiceBusInMemory();
            app.RegisterBrickServiceBus();

            // Logging Brick
            app.RegisterBrickLoggingApi();
            app.RegisterBrickLoggingApiController();
            app.RegisterBrickLoggingEntityFrameworkCoreInMemory();
            app.RegisterBrickLogging();

            // Security Brick
            app.RegisterBrickSecurityApi();
            app.RegisterBrickSecurityApiController();
            app.RegisterBrickSecurityEntityFrameworkCoreInMemory();
            app.RegisterBrickSecurity();

            // Example Brick
            app.RegisterBrickExampleApi();
            app.RegisterBrickExampleApiController();
            app.RegisterBrickExampleEntityFrameworkCoreInMemory();
            app.RegisterBrickExample();

            // Custom Website
            app.RegisterCustomWebsite();
        }

        public override void StartBricks(IApplicationBuilder app)
        {
            // Call base implementation
            base.StartBricks(app);

            // Service Brick Core
            app.StartBrickCore();

            // Service Bus Brick
            app.StartBrickServiceBusInMemory();
            app.StartBrickServiceBus();

            // Logging Brick
            app.StartBrickLoggingApi();
            app.StartBrickLoggingApiController();
            app.StartBrickLoggingEntityFrameworkCoreInMemory();
            app.StartBrickLogging();

            // Security Brick
            app.StartBrickSecurityApi();
            app.StartBrickSecurityApiController();
            app.StartBrickSecurityEntityFrameworkCoreInMemory();
            app.StartBrickSecurity();

            // Example Brick
            app.StartBrickExampleApi();
            app.StartBrickExampleApiController();
            app.StartBrickExampleEntityFrameworkCoreInMemory();
            app.StartBrickExample();

            // Custom Website
            if (WebHostEnvironment != null)
                app.StartCustomWebsite(WebHostEnvironment);
        }
    }
}