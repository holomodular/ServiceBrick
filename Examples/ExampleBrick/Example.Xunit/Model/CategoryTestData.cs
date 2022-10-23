using Example.Api;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;
using ServiceBrick.Xunit;
using ServiceQuery;

namespace Example.Xunit
{
    public class CategoryTestData : TestData<CategoryDto>
    {
        public override CategoryDto GetMinimumDataObject()
        {
            return new CategoryDto()
            {
                Name = Guid.NewGuid().ToString(),
            };
        }

        public override CategoryDto GetMaximumDataObject()
        {
            var model = new CategoryDto()
            {
                Name = Guid.NewGuid().ToString(),
                CreateDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
            };
            return model;
        }

        public override CategoryDto GetObjectNotFound()
        {
            return new CategoryDto()
            {
                StorageKey = "-999"
            };
        }

        public override IApiController<CategoryDto> GetController(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ICategoryApiController>();
        }

        public override IApiClient<CategoryDto> GetClient(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ICategoryApiClient>();
        }

        public override IApiService<CategoryDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ICategoryApiService>();
        }

        public override void UpdateObject(CategoryDto dto)
        {
            dto.Name = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(CategoryDto clientDto, CategoryDto serviceDto, HttpMethod method)
        {
            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate > clientDto.CreateDate);
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);

            //UpdateDateRule
            if (method == HttpMethod.Post || method == HttpMethod.Put)
                Assert.True(serviceDto.UpdateDate > clientDto.UpdateDate); //Rule
            else
                Assert.True(serviceDto.UpdateDate == clientDto.UpdateDate);

            Assert.True(serviceDto.Name == clientDto.Name);
        }

        public override List<Query> GetQueriesForObject(CategoryDto dto)
        {
            List<Query> queries = new List<Query>();

            var qb = QueryBuilder.New().
                IsEqual(nameof(ProductDto.CreateDate), dto.CreateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = QueryBuilder.New().
                IsEqual(nameof(ProductDto.UpdateDate), dto.UpdateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = QueryBuilder.New().
                IsEqual(nameof(ProductDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = QueryBuilder.New().
                IsEqual(nameof(ProductDto.Name), dto.Name);
            queries.Add(qb.Build());

            return queries;
        }
    }
}