using ServiceBrick;
using ServiceBrick.Logging;
using ServiceBrick.Logging.Api;
using ServiceBrick.Logging.MongoDb;
using ServiceBrick.Notification;
using ServiceBrick.Notification.Api;
using ServiceBrick.Notification.Api.Controller;
using ServiceBrick.Notification.MongoDb;
using ServiceBrick.Security.Member;
using ServiceBrick.ServiceBus;
using ServiceBrick.ServiceBus.Azure;
using WebApp.Extensions;

namespace WebApp
{
    /// <summary>
    /// This class starts SERVICE BRICK using the MongoDB provider.
    /// Change Program.cs to use this class.
    /// </summary>
    public class StartupServiceBrickMongoDb : ServiceBrick.Startup
    {
        public StartupServiceBrickMongoDb(IConfiguration configuration) : base(configuration)
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
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupServiceBrickMongoDb>>();
            logger.LogInformation("Application Started");
        }

        public override void AddBricks(IServiceCollection services)
        {
            // Call base implementation
            base.AddBricks(services);

            // Service Brick Core
            services.AddBrickCore(Configuration, typeof(StartupServiceBrickMongoDb).Assembly);

            // Service Bus Brick
            services.AddBrickServiceBusAzure(Configuration);
            services.AddBrickServiceBus(Configuration);

            // Logging Brick
            services.AddBrickLoggingApi(Configuration);
            services.AddBrickLoggingMongoDb(Configuration);
            services.AddBrickLogging(Configuration);

            // Notification Brick
            services.AddBrickNotificationApi(Configuration);
            services.AddBrickNotificationApiController(Configuration);
            services.AddBrickNotificationMongoDb(Configuration);
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
            app.RegisterBrickLoggingMongoDb();
            app.RegisterBrickLogging();

            // Notification Brick
            app.RegisterBrickNotificationApi();
            app.RegisterBrickNotificationApiController();
            app.RegisterBrickNotificationMongoDb();
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
            app.StartBrickLoggingMongoDb();
            app.StartBrickLogging();

            // Notification Brick
            app.StartBrickNotificationApi();
            app.StartBrickNotificationApiController();
            app.StartBrickNotificationMongoDb();
            app.StartBrickNotification();

            // Security Member Brick
            app.StartBrickSecurityMember();

            // Custom Website
            if (WebHostEnvironment != null)
                app.StartCustomWebsite(WebHostEnvironment);
        }
    }
}