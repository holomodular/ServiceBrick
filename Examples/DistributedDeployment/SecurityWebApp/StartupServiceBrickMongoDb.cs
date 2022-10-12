using ServiceBrick;

using ServiceBrick.Logging;
using ServiceBrick.Logging.Api;
using ServiceBrick.Logging.MongoDb;
using ServiceBrick.Security;
using ServiceBrick.Security.Api;
using ServiceBrick.Security.Api.Controller;
using ServiceBrick.Security.MongoDb;
using ServiceBrick.ServiceBus;
using ServiceBrick.ServiceBus.Azure;

//using ServiceBrick.Web.Admin;
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
            app.RegisterBrickServiceBusAzure();
            app.RegisterBrickServiceBus();

            // Logging Brick
            app.RegisterBrickLoggingApi();
            app.RegisterBrickLoggingMongoDb();
            app.RegisterBrickLogging();

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
            app.StartBrickServiceBusAzure();
            app.StartBrickServiceBus();

            // Logging Brick
            app.StartBrickLoggingApi();
            app.StartBrickLoggingMongoDb();
            app.StartBrickLogging();

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