using Microsoft.AspNetCore.Mvc;
using ServiceBrick;

namespace Example.Api
{
    public interface IProductApiController : IApiController<ProductDto>
    {
        Task<ActionResult> AddCategory([FromQuery] string productStorageKey, [FromQuery] string categoryStorageKey);

        Task<ActionResult> RemoveCategory([FromQuery] string productStorageKey, [FromQuery] string categoryStorageKey);
    }
}