using Microsoft.AspNetCore.Mvc;
using ServiceBrick;

namespace Example.Api.Controller
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Example/Category")]
    [Produces("application/json")]
    public class CategoryApiController : AdminPolicyApiController<CategoryDto>, ICategoryApiController
    {
        protected readonly ICategoryApiService _categoryApiService;

        public CategoryApiController(
            ICategoryApiService categoryApiService)
            : base(categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }
    }
}