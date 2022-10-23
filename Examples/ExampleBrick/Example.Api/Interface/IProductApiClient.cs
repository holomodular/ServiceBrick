using ServiceBrick;

namespace Example.Api
{
    public interface IProductApiClient : IApiClient<ProductDto>, IProductApiService
    {
    }
}