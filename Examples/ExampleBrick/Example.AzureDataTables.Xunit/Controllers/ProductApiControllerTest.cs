using Example.Api;
using ServiceBrick.Xunit;

namespace Example.AzureDataTables.Xunit
{
    [Collection("AzureDataTables")]
    public class ProductApiControllerTest : Example.Xunit.ProductApiControllerTest, IDisposable
    {
        public ProductApiControllerTest() : base()
        {
            SystemManager?.StartSystem(typeof(ExampleStartupAzureDataTables));
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