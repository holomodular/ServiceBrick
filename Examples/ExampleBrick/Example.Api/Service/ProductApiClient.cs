using Microsoft.Extensions.Configuration;
using RestSharp;
using ServiceBrick;

namespace Example.Api
{
    public class ProductApiClient : ApiClient<ProductDto>, IProductApiClient
    {
        protected readonly IConfiguration _configuration;

        public ProductApiClient(
            IConfiguration configuration)
            : base(configuration.GetApiConfig(ExampleApiConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"/api/v1.0/Example/Product";
        }

        public ProductApiClient(ApiConfig apiConfig) : base(apiConfig)
        {
            ApiResource = @"/api/v1.0/Example/Product";
        }

        public async Task<IResponse> AddCategoryAsync(string productStorageKey, string categoryStorageKey)
        {
            RestRequest req = new RestRequest();
            req.Method = Method.Post;
            req.Resource = $"{ApiResource}/AddCategory";
            req.AddQueryParameter("productStorageKey", productStorageKey);
            req.AddQueryParameter("categoryStorageKey", categoryStorageKey);
            return await ExecuteAsync<bool>(req);
        }

        public async Task<IResponse> RemoveCategoryAsync(string productStorageKey, string categoryStorageKey)
        {
            RestRequest req = new RestRequest();
            req.Method = Method.Delete;
            req.Resource = $"{ApiResource}/RemoveCategory";
            req.AddQueryParameter("productStorageKey", productStorageKey);
            req.AddQueryParameter("categoryStorageKey", categoryStorageKey);
            return await ExecuteAsync<bool>(req);
        }
    }
}