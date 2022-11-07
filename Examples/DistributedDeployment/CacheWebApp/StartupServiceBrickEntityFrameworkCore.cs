using ServiceBrick;
using ServiceBrick.Cache;
using ServiceBrick.Cache.Api;
using ServiceBrick.Cache.Api.Controller;
using ServiceBrick.Cache.EntityFrameworkCore;
using ServiceBrick.Logging;
using ServiceBrick.Logging.Api;
using ServiceBrick.Logging.EntityFrameworkCore;
using ServiceBrick.Security.Member;
using ServiceBrick.ServiceBus;
using ServiceBrick.ServiceBus.Azure;
using WebApp.Extensions;

namespace WebApp
{
    /// <summary>
    /// This class starts SERVICE BRICK using EntityFrameworkCore provider (SqlServer)
    /// Change Program.cs to use this class.
    /// </summary>
    public class StartupServiceBrickEntityFrameworkCore : ServiceBrick.Startup
    {
        public StartupServiceBrickEntityFrameworkCore(IConfiguration configuration) : base(configuration)
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
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupServiceBrickEntityFrameworkCore>>();
            logger.LogInformation("Application Started");
        }

        public override void AddBricks(IServiceCollection services)
        {
            // Call base implementation
            base.AddBricks(services);

            // Service Brick Core
            services.AddBrickCore(Configuration, typeof(StartupServiceBrickEntityFrameworkCore).Assembly);

            // Service Bus Brick
            services.AddBrickServiceBusAzure(Configuration);
            services.AddBrickServiceBus(Configuration);

            // Logging Brick
            services.AddBrickLoggingApi(Configuration);
            services.AddBrickLoggingEntityFrameworkCore(Configuration);
            services.AddBrickLogging(Configuration);

            // Security Member Brick
            services.AddBrickSecurityMember(Configuration);

            // Cache Brick
            services.AddBrickCacheApi(Configuration);
            services.AddBrickCacheApiController(Configuration);
            services.AddBrickCacheEntityFrameworkCore(Configuration);
            services.AddBrickCache(Configuration);

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
            app.RegisterBrickServiceBusAzure();
            app.RegisterBrickServiceBus();

            // Logging Brick
            app.RegisterBrickLoggingApi();
            app.RegisterBrickLoggingEntityFrameworkCore();
            app.RegisterBrickLogging();

            // Security Member Brick
            app.RegisterBrickSecurityMember();

            // Cache Brick
            app.RegisterBrickCacheApi();
            app.RegisterBrickCacheApiController();
            app.RegisterBrickCacheEntityFrameworkCore();
            app.RegisterBrickCache();

            // Custom Website
            app.RegisterCustomWebsite();
        }

        public override void StartBricks(IApplicationBuilder app)
        {
            // Call base implementation
            base.StartBricks(app);

            // Service Brick Core
            app.StartBrickCore();

            // Logging Brick
            app.StartBrickLoggingApi();
            app.StartBrickLoggingEntityFrameworkCore();
            app.StartBrickLogging();

            // Security Member Brick
            app.StartBrickSecurityMember();

            // Cache Brick
            app.StartBrickCacheApi();
            app.StartBrickCacheApiController();
            app.StartBrickCacheEntityFrameworkCore();
            app.StartBrickCache();

            // Service Bus Brick
            app.StartBrickServiceBusAzure();
            app.StartBrickServiceBus();

            // Custom Website
            if (WebHostEnvironment != null)
                app.StartCustomWebsite(WebHostEnvironment);
        }
    }
}