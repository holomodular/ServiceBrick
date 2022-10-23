using Example.Api;
using Example.Api.Controller;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;

namespace Example.EntityFrameworkCore.Xunit
{
    public class ExampleStartupEntityFrameworkCoreInMemory : ServiceBrick.Startup
    {
        public ExampleStartupEntityFrameworkCoreInMemory(IWebHostEnvironment env) : base(null)
        {
            Configuration = new ConfigurationBuilder()
            .AddAppSettingsConfig()
            .Build();
        }

        public virtual void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            base.CustomConfigureServices(services);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            base.CustomConfigure(app);
        }

        public override void AddBricks(IServiceCollection services)
        {
            base.AddBricks(services);

            services.AddBrickCore(Configuration, typeof(ExampleStartupEntityFrameworkCore).Assembly);
            services.AddBrickExampleApi(Configuration);
            services.AddBrickExampleApiController(Configuration);
            services.AddBrickExampleEntityFrameworkCoreInMemory(Configuration);
            services.AddBrickExample(Configuration);

            // Remove all background tasks/timers for testing
            var timer = services.Where(x => x.ImplementationType == typeof(ExampleExpireProductTimer)).FirstOrDefault();
            if (timer != null)
                services.Remove(timer);
        }

        public override void RegisterBricks(IApplicationBuilder app)
        {
            base.RegisterBricks(app);

            app.RegisterBrickCore();
            app.RegisterBrickExampleApi();
            app.RegisterBrickExampleApiController();
            app.RegisterBrickExampleEntityFrameworkCoreInMemory();
            app.RegisterBrickExample();
        }

        public override void StartBricks(IApplicationBuilder app)
        {
            base.StartBricks(app);

            app.StartBrickCore();
            app.StartBrickExampleApi();
            app.StartBrickExampleApiController();
            app.StartBrickExampleEntityFrameworkCoreInMemory();
            app.StartBrickExample();
        }
    }
}