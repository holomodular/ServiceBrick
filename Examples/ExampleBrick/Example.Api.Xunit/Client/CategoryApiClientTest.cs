using Example.Xunit;
using ServiceBrick.Xunit;

namespace Example.Api.Xunit
{
    [Collection("Api")]
    public class CategoryApiClientTest : Example.Xunit.CategoryApiClientTest, IDisposable
    {
        public CategoryApiClientTest()
        {
            SystemManager = new SystemManager();
            TestData = new CategoryTestData();
            SystemManager?.StartSystem(typeof(ExampleStartupApi));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}