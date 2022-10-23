using Example.Api;
using Microsoft.Extensions.DependencyInjection;
using ServiceBrick;
using ServiceBrick.Xunit;
using ServiceQuery;

namespace Example.Xunit
{
    public class ProductTestData : TestData<ProductDto>
    {
        public override ProductDto GetMinimumDataObject()
        {
            return new ProductDto()
            {
                Name = Guid.NewGuid().ToString(),
                ExpirationDate = DateTimeOffset.UtcNow.AddDays(1),
            };
        }

        public override ProductDto GetMaximumDataObject()
        {
            var model = new ProductDto()
            {
                Name = Guid.NewGuid().ToString(),
                IsExpired = false,
                CreateDate = DateTimeOffset.UtcNow,
                ExpirationDate = DateTimeOffset.UtcNow.AddDays(1),
                UpdateDate = DateTimeOffset.UtcNow,
            };
            return model;
        }

        public override ProductDto GetObjectNotFound()
        {
            return new ProductDto()
            {
                StorageKey = "-999"
            };
        }

        public override IApiController<ProductDto> GetController(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IProductApiController>();
        }

        public override IApiClient<ProductDto> GetClient(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IProductApiClient>();
        }

        public override IApiService<ProductDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IProductApiService>();
        }

        public override void UpdateObject(ProductDto dto)
        {
            dto.Name = Guid.NewGuid().ToString();
            dto.ExpirationDate = DateTimeOffset.UtcNow.AddDays(-1);
            dto.IsExpired = true;
        }

        public override void ValidateObjects(ProductDto clientDto, ProductDto serviceDto, HttpMethod method)
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

            Assert.True(serviceDto.ExpirationDate == clientDto.ExpirationDate);

            Assert.True(serviceDto.IsExpired == clientDto.IsExpired);
        }

        public override List<Query> GetQueriesForObject(ProductDto dto)
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

            qb = QueryBuilder.New().
                IsEqual(nameof(ProductDto.IsExpired), dto.IsExpired.ToString());
            queries.Add(qb.Build());

            qb = QueryBuilder.New().
                IsEqual(nameof(ProductDto.ExpirationDate), dto.ExpirationDate.ToString("o"));
            queries.Add(qb.Build());

            return queries;
        }
    }
}