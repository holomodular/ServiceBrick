using ServiceBrick;
using ServiceBrick.Logging;
using ServiceBrick.Logging.Api;
using ServiceBrick.Logging.AzureDataTables;
using ServiceBrick.Notification;
using ServiceBrick.Notification.Api;
using ServiceBrick.Notification.Api.Controller;
using ServiceBrick.Notification.AzureDataTables;
using ServiceBrick.Security.Member;
using ServiceBrick.ServiceBus;
using ServiceBrick.ServiceBus.Azure;
using WebApp.Extensions;

namespace WebApp
{
    /// <summary>
    /// This class starts SERVICE BRICK using the AzureDataTables provider.
    /// Change Program.cs to use this class.
    /// </summary>
    public class StartupServiceBrickAzureDataTables : ServiceBrick.Startup
    {
        public StartupServiceBrickAzureDataTables(IConfiguration configuration) : base(configuration)
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
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupServiceBrickAzureDataTables>>();
            logger.LogInformation("Application Started");
        }

        public override void AddBricks(IServiceCollection services)
        {
            // Call base implementation
            base.AddBricks(services);

            // Service Brick Core
            services.AddBrickCore(Configuration, typeof(StartupServiceBrickAzureDataTables).Assembly);

            // Service Bus Brick
            services.AddBrickServiceBusAzure(Configuration);
            services.AddBrickServiceBus(Configuration);

            // Logging Brick
            services.AddBrickLoggingApi(Configuration);
            services.AddBrickLoggingAzureDataTables(Configuration);
            services.AddBrickLogging(Configuration);

            // Notification Brick
            services.AddBrickNotificationApi(Configuration);
            services.AddBrickNotificationApiController(Configuration);
            services.AddBrickNotificationAzureDataTables(Configuration);
            services.AddBrickNotification(Configuration);

            // Security Member Brick
            services.AddBrickSecurityMember(Configuration);

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
            app.RegisterBrickLoggingAzureDataTables();
            app.RegisterBrickLogging();

            // Notification Brick
            app.RegisterBrickNotificationApi();
            app.RegisterBrickNotificationApiController();
            app.RegisterBrickNotificationAzureDataTables();
            app.RegisterBrickNotification();

            // Security Member Brick
            app.RegisterBrickSecurityMember();

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
            app.StartBrickServiceBusAzure();
            app.StartBrickServiceBus();

            // Logging Brick
            app.StartBrickLoggingApi();
            app.StartBrickLoggingAzureDataTables();
            app.StartBrickLogging();

            // Notification Brick
            app.StartBrickNotificationApi();
            app.StartBrickNotificationApiController();
            app.StartBrickNotificationAzureDataTables();
            app.StartBrickNotification();

            // Security Member Brick
            app.StartBrickSecurityMember();

            // Custom Website
            if (WebHostEnvironment != null)
                app.StartCustomWebsite(WebHostEnvironment);
        }
    }
}