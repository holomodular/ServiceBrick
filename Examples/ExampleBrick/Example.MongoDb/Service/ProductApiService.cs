using AutoMapper;
using Example.Api;
using ServiceBrick;
using ServiceQuery;

namespace Example.MongoDb
{
    public class ProductApiService : ApiService<Product, ProductDto>, IProductApiService
    {
        protected readonly IDomainRepository<ProductCategory> _productCategoryRepo;

        public ProductApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<Product> repository,
            IDomainRepository<ProductCategory> productCategoryRepo)
            : base(mapper, businessRuleService, repository)
        {
            _productCategoryRepo = productCategoryRepo;
        }

        public async Task<IResponse> AddCategoryAsync(string productStorageKey, string categoryStorageKey)
        {
            Response response = new Response();

            // Create query
            var query = QueryBuilder.New()
                .IsEqual(nameof(ProductCategory.ProductId), productStorageKey)
                .And()
                .IsEqual(nameof(ProductCategory.CategoryId), categoryStorageKey)
                .Build();

            // Query
            var respQ = await _productCategoryRepo.QueryAsync(query);
            if (respQ.Error)
            {
                response.CopyFrom(respQ);
                return response;
            }
            if (respQ.List.Count > 0)
            {
                response.AddMessage(ResponseMessage.CreateError("Already exists"));
                return response;
            }

            // Create
            ProductCategory pc = new ProductCategory()
            {
                ProductId = productStorageKey,
                CategoryId = categoryStorageKey
            };
            var respCreate = await _productCategoryRepo.CreateAsync(pc);
            response.CopyFrom(respCreate);

            return response;
        }

        public async Task<IResponse> RemoveCategoryAsync(string productStorageKey, string categoryStorageKey)
        {
            Response response = new Response();

            // Create query
            var query = QueryBuilder.New()
                .IsEqual(nameof(ProductCategory.ProductId), productStorageKey)
                .And()
                .IsEqual(nameof(ProductCategory.CategoryId), categoryStorageKey)
                .Build();

            // Query
            var respQ = await _productCategoryRepo.QueryAsync(query);
            if (respQ.Error)
            {
                response.CopyFrom(respQ);
                return response;
            }
            if (respQ.List.Count == 0)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            // Delete
            var respDelete = await _productCategoryRepo.DeleteAsync(respQ.List[0]);
            response.CopyFrom(respDelete);
            return response;
        }
    }
}