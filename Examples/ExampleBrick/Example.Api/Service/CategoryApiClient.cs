using Microsoft.Extensions.Configuration;
using ServiceBrick;

namespace Example.Api
{
    public class CategoryApiClient : ApiClient<CategoryDto>, ICategoryApiClient
    {
        protected readonly IConfiguration _configuration;

        public CategoryApiClient(
            IConfiguration configuration)
            : base(configuration.GetApiConfig(ExampleApiConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"/api/v1.0/Example/Category";
        }

        public CategoryApiClient(ApiConfig apiConfig) : base(apiConfig)
        {
            ApiResource = @"/api/v1.0/Example/Category";
        }
    }
}