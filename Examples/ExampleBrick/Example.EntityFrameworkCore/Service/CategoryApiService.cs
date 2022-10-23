using AutoMapper;
using Example.Api;
using ServiceBrick;

namespace Example.EntityFrameworkCore
{
    public class CategoryApiService : ApiService<Category, CategoryDto>, ICategoryApiService
    {
        protected readonly ExampleContext _exampleContext;

        public CategoryApiService(
            IMapper mapper,
            IBusinessRuleService businessRuleService,
            IDomainRepository<Category> repository,
            ExampleContext exampleContext)
            : base(mapper, businessRuleService, repository)
        {
            _exampleContext = exampleContext;
        }
    }
}