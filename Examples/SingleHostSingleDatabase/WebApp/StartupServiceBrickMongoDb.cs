using ServiceBrick;
using ServiceBrick.Cache;
using ServiceBrick.Cache.Api;
using ServiceBrick.Cache.Api.Controller;
using ServiceBrick.Cache.MongoDb;
using ServiceBrick.Logging;
using ServiceBrick.Logging.Api;
using ServiceBrick.Logging.Api.Controller;
using ServiceBrick.Logging.MongoDb;
using ServiceBrick.Notification;
using ServiceBrick.Notification.Api;
using ServiceBrick.Notification.Api.Controller;
using ServiceBrick.Notification.MongoDb;
using ServiceBrick.Security;
using ServiceBrick.Security.Api;
using ServiceBrick.Security.Api.Controller;
using ServiceBrick.Security.MongoDb;
using ServiceBrick.ServiceBus;
using ServiceBrick.ServiceBus.InMemory;
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
            services.AddBrickServiceBusInMemory(Configuration);
            services.AddBrickServiceBus(Configuration);

            // Logging Brick
            services.AddBrickLoggingApi(Configuration);
            services.AddBrickLoggingApiController(Configuration);
            services.AddBrickLoggingMongoDb(Configuration);
            services.AddBrickLogging(Configuration);

            // Cache Brick
            services.AddBrickCacheApi(Configuration);
            services.AddBrickCacheApiController(Configuration);
            services.AddBrickCacheMongoDb(Configuration);
            services.AddBrickCache(Configuration);

            // Notification Brick
            services.AddBrickNotificationApi(Configuration);
            services.AddBrickNotificationApiController(Configuration);
            services.AddBrickNotificationMongoDb(Configuration);
            services.AddBrickNotification(Configuration);

            // Security Brick
            services.AddBrickSecurityApi(Configuration);
            services.AddBrickSecurityApiController(Configuration);
            services.AddBrickSecurityMongoDb(Configuration);
            services.AddBrickSecurity(Configuration);

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
            app.RegisterBrickLoggingMongoDb();
            app.RegisterBrickLogging();

            // Cache Brick
            app.RegisterBrickCacheApi();
            app.RegisterBrickCacheApiController();
            app.RegisterBrickCacheMongoDb();
            app.RegisterBrickCache();

            // Notification Brick
            app.RegisterBrickNotificationApi();
            app.RegisterBrickNotificationApiController();
            app.RegisterBrickNotificationMongoDb();
            app.RegisterBrickNotification();

            // Security Brick
            app.RegisterBrickSecurityApi();
            app.RegisterBrickSecurityApiController();
            app.RegisterBrickSecurityMongoDb();
            app.RegisterBrickSecurity();

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
            app.StartBrickLoggingMongoDb();
            app.StartBrickLogging();

            // Cache Brick
            app.StartBrickCacheApi();
            app.StartBrickCacheApiController();
            app.StartBrickCacheMongoDb();
            app.StartBrickCache();

            // Notification Brick
            app.StartBrickNotificationApi();
            app.StartBrickNotificationApiController();
            app.StartBrickNotificationMongoDb();
            app.StartBrickNotification();

            // Security Brick
            app.StartBrickSecurityApi();
            app.StartBrickSecurityApiController();
            app.StartBrickSecurityMongoDb();
            app.StartBrickSecurity();

            // Custom Website
            if (WebHostEnvironment != null)
                app.StartCustomWebsite(WebHostEnvironment);
        }
    }
}