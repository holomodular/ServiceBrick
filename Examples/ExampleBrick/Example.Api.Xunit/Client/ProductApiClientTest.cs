using Example.Xunit;
using ServiceBrick.Xunit;

namespace Example.Api.Xunit
{
    [Collection("Api")]
    public class ProductApiClientTest : Example.Xunit.ProductApiClientTest, IDisposable
    {
        public ProductApiClientTest()
        {
            SystemManager = new SystemManager();
            TestData = new ProductTestData();
            SystemManager?.StartSystem(typeof(ExampleStartupApi));
        }

        public override ApiClientTest<CategoryDto> GetCategoryClient()
        {
            return new CategoryApiClientTest();
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}