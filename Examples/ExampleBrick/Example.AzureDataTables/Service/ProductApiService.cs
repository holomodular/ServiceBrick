using AutoMapper;
using Example.Api;
using ServiceBrick;

namespace Example.AzureDataTables
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

            ProductCategory pc = new ProductCategory();
            pc.PartitionKey = productStorageKey;
            pc.RowKey = categoryStorageKey;
            var respGetItem = await _productCategoryRepo.GetItemAsync(pc);
            if (respGetItem.Error)
            {
                response.CopyFrom(respGetItem);
                return response;
            }
            if (respGetItem.Item == null)
            {
                var respCreate = await _productCategoryRepo.CreateAsync(pc);
                response.CopyFrom(respCreate);
            }
            else
                response.AddMessage(ResponseMessage.CreateError("Already exists"));
            return response;
        }

        public async Task<IResponse> RemoveCategoryAsync(string productStorageKey, string categoryStorageKey)
        {
            Response response = new Response();

            ProductCategory pc = new ProductCategory();
            pc.PartitionKey = productStorageKey;
            pc.RowKey = categoryStorageKey;

            // Delete it
            var respDelete = await _productCategoryRepo.DeleteAsync(pc);
            response.CopyFrom(respDelete);

            return response;
        }
    }
}