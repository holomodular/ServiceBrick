using ServiceBrick;

namespace Example.Api
{
    public interface IProductApiService : IApiService<ProductDto>
    {
        Task<IResponse> AddCategoryAsync(string productStorageKey, string categoryStorageKey);

        Task<IResponse> RemoveCategoryAsync(string productStorageKey, string categoryStorageKey);
    }
}