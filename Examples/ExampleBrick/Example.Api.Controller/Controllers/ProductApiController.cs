using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceBrick;
using System.Net;

namespace Example.Api.Controller
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Example/Product")]
    [Produces("application/json")]
    public class ProductApiController : AdminPolicyApiController<ProductDto>, IProductApiController
    {
        protected readonly IProductApiService _productApiService;

        public ProductApiController(
            IProductApiService productApiService)
            : base(productApiService)
        {
            _productApiService = productApiService;
        }

        [HttpPost]
        [Route("AddCategory")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBrickConstants.AdminSecurityPolicyName)]
        public async Task<ActionResult> AddCategory([FromQuery] string productStorageKey, [FromQuery] string categoryStorageKey)
        {
            var resp = await _productApiService.AddCategoryAsync(productStorageKey, categoryStorageKey);
            if (resp.Success)
                return Ok(true);
            return GetErrorResponse(resp);
        }

        [HttpDelete]
        [Route("RemoveCategory")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Policy = ServiceBrickConstants.AdminSecurityPolicyName)]
        public async Task<ActionResult> RemoveCategory([FromQuery] string productStorageKey, [FromQuery] string categoryStorageKey)
        {
            var resp = await _productApiService.RemoveCategoryAsync(productStorageKey, categoryStorageKey);
            if (resp.Success)
                return Ok(true);
            return GetErrorResponse(resp);
        }
    }
}