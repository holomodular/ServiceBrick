using Example.Api;
using ServiceBrick.Xunit;

namespace Example.MongoDb.Xunit
{
    [Collection("MongoDb")]
    public class ProductApiControllerTest : Example.Xunit.ProductApiControllerTest, IDisposable
    {
        public ProductApiControllerTest() : base()
        {
            TestData = new ProductTestData();
            SystemManager?.StartSystem(typeof(ExampleStartupMongoDb));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }

        public override ApiControllerTest<CategoryDto> GetCategoryController()
        {
            return new CategoryApiControllerTest();
        }
    }
}