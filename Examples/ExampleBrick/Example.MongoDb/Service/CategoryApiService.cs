using AutoMapper;
using Example.Api;
using ServiceBrick;

namespace Example.MongoDb
{
    public class CategoryApiService : ApiService<Category, CategoryDto>, ICategoryApiService
    {
        public CategoryApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<Category> repository)
            : base(mapper, businessRuleService, repository)
        {
        }
    }
}