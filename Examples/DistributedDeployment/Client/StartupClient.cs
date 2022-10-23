using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;
using ServiceBrick.Cache.Api;
using ServiceBrick.Logging.Api;
using ServiceBrick.Notification.Api;
using ServiceBrick.Security.Api;

namespace Client
{
    public class StartupClient : ServiceBrick.Startup
    {
        public StartupClient(IConfiguration configuration) : base(configuration)
        { }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
        }

        public override void AddBricks(IServiceCollection services)
        {
            base.AddBricks(services);

            services.AddBrickCore(Configuration, typeof(StartupClient).Assembly);
            services.AddBrickLoggingApi(Configuration);
            services.AddBrickCacheApi(Configuration);
            services.AddBrickNotificationApi(Configuration);
            services.AddBrickSecurityApi(Configuration);
        }

        public override void RegisterBricks(IApplicationBuilder app)
        {
            base.RegisterBricks(app);

            app.RegisterBrickCore();
            app.RegisterBrickLoggingApi();
            app.RegisterBrickCacheApi();
            app.RegisterBrickNotificationApi();
            app.RegisterBrickSecurityApi();
        }

        public override void StartBricks(IApplicationBuilder app)
        {
            base.StartBricks(app);

            app.StartBrickCore();
            app.StartBrickLoggingApi();
            app.StartBrickCacheApi();
            app.StartBrickNotificationApi();
            app.StartBrickSecurityApi();
        }
    }
}