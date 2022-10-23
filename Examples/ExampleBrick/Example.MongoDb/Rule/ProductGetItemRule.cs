using Example.Api;
using Microsoft.Extensions.Logging;
using ServiceBrick;
using ServiceQuery;

namespace Example.MongoDb
{
    public partial class ProductGetItemRule : BusinessRule
    {
        protected readonly ILogger _logger;
        protected readonly IDomainRepository<ProductCategory> _productCategoryRepo;
        protected readonly ICategoryApiService _categoryApiService;

        public ProductGetItemRule(
            ILoggerFactory loggerFactory,
            IDomainRepository<ProductCategory> productCategoryRepo,
            ICategoryApiService categoryApiService)
        {
            _logger = loggerFactory.CreateLogger<ProductGetItemRule>();
            _productCategoryRepo = productCategoryRepo;
            _categoryApiService = categoryApiService;
            Priority = PRIORITY_NORMAL;
        }

        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(ApiGetItemAfterEvent<ProductDto>),
                typeof(ProductGetItemRule));
        }

        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            try
            {
                if (context.Object is ApiGetItemAfterEvent<ProductDto> de)
                {
                    if (de.DomainObject == null)
                        return new Response();
                    var item = de.DomainObject;

                    var query = QueryBuilder.New()
                        .IsEqual(nameof(ProductCategory.ProductId), item.StorageKey)
                        .Build();
                    var respQ = await _productCategoryRepo.QueryAsync(query);
                    if (respQ.Success && respQ.List.Count > 0)
                    {
                        var categories = respQ.List.Select(x => x.CategoryId).ToArray();
                        query = QueryBuilder.New()
                            .IsInSet(nameof(Category.Id), categories)
                            .Build();
                        var respCats = await _categoryApiService.QueryAsync(query);
                        if (respCats.Success)
                            de.DomainObject.Categories = respCats.List;
                    }

                    if (item.Categories == null)
                        item.Categories = new List<CategoryDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                // Don't return an error in an after event, unless you plan on deleting the item
            }

            return new Response();
        }
    }
}