using ServiceBrick;

namespace Example.Api
{
    public interface ICategoryApiClient : IApiClient<CategoryDto>, ICategoryApiService
    {
    }
}