using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;

namespace Example.Api.Xunit
{
    public class ExampleStartupApi : ServiceBrick.Startup
    {
        public ExampleStartupApi(IWebHostEnvironment env) : base(null)
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

            services.AddBrickCore(Configuration, typeof(ExampleStartupApi).Assembly);
            services.AddBrickExampleApi(Configuration);
        }

        public override void RegisterBricks(IApplicationBuilder app)
        {
            base.RegisterBricks(app);

            app.RegisterBrickCore();
            app.RegisterBrickExampleApi();
        }

        public override void StartBricks(IApplicationBuilder app)
        {
            base.StartBricks(app);

            app.StartBrickCore();
            app.StartBrickExampleApi();
        }
    }
}