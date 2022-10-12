using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApp.Model;

namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomWebsite(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddOptions();
            services.AddCors();
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options =>
                {
                    // Added for ServiceQuery enums in swagger
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.AddCustomSwagger(Configuration);

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader();
            });
            services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(descriptions =>
                {
                    return descriptions.First();
                });
                options.CustomSchemaIds(x => x.FullName);
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "1.0" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "API v2", Version = "2.0" });
                options.OperationFilter<SwaggerRemoveVersionOperationFilter>();
                options.DocumentFilter<SwaggerReplaceVersionDocumentFilter>();
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    return docName == apiDesc.GroupName;
                });
            });
            return services;
        }
    }
}