using AutoMapper;
using Example.Api;
using Microsoft.Extensions.Logging;
using ServiceBrick;

namespace Example.EntityFrameworkCore
{
    public class ProductApiService : ApiService<Product, ProductDto>, IProductApiService
    {
        protected readonly ExampleContext _exampleContext;
        protected readonly IDomainRepository<ProductCategory> _productCategoryRepository;
        protected readonly ILogger<ProductApiService> _logger;

        public ProductApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<Product> repository,
            ExampleContext exampleContext,
            IDomainRepository<ProductCategory> productCategoryRepository,
            ILogger<ProductApiService> logger)
            : base(mapper, businessRuleService, repository)
        {
            _exampleContext = exampleContext;
            _productCategoryRepository = productCategoryRepository;
            _logger = logger;
        }

        public async Task<IResponse> AddCategoryAsync(string productStorageKey, string categoryStorageKey)
        {
            Response response = new Response();
            int productId;
            if (!int.TryParse(productStorageKey, out productId))
            {
                response.AddMessage(ResponseMessage.CreateError("Invalid productStorageKey"));
                return response;
            }
            int categoryId;
            if (!int.TryParse(categoryStorageKey, out categoryId))
            {
                response.AddMessage(ResponseMessage.CreateError("Invalid categoryStorageKey"));
                return response;
            }

            var existingRecord = _exampleContext.ProductCategory.Where(x =>
                    x.ProductID == productId &&
                    x.CategoryID == categoryId).FirstOrDefault();
            if (existingRecord != null)
            {
                response.AddMessage(ResponseMessage.CreateError("Association already exists"));
                return response;
            }

            // Add record
            ProductCategory pc = new ProductCategory();
            pc.ProductID = productId;
            pc.CategoryID = categoryId;

            try
            {
                _exampleContext.ProductCategory.Add(pc);
                await _exampleContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
            }

            return response;
        }

        public async Task<IResponse> RemoveCategoryAsync(string productStorageKey, string categoryStorageKey)
        {
            Response response = new Response();
            int productId;
            if (!int.TryParse(productStorageKey, out productId))
            {
                response.AddMessage(ResponseMessage.CreateError("Invalid productStorageKey"));
                return response;
            }
            int categoryId;
            if (!int.TryParse(categoryStorageKey, out categoryId))
            {
                response.AddMessage(ResponseMessage.CreateError("Invalid productStorageKey"));
                return response;
            }

            var existingRecord = _exampleContext.ProductCategory.Where(x =>
                    x.ProductID == productId &&
                    x.CategoryID == categoryId).FirstOrDefault();
            if (existingRecord == null)
            {
                response.AddMessage(ResponseMessage.CreateError("Association does not exist"));
                return response;
            }

            // Remove record
            try
            {
                _exampleContext.ProductCategory.Remove(existingRecord);
                await _exampleContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
            }

            return response;
        }
    }
}