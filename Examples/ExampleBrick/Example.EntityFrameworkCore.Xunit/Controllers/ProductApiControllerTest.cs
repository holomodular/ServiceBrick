using Example.Api;
using ServiceBrick.Xunit;

namespace Example.EntityFrameworkCore.Xunit
{
    [Collection("EntityFrameworkCore")]
    public class ProductApiControllerTest : Example.Xunit.ProductApiControllerTest, IDisposable
    {
        public ProductApiControllerTest() : base()
        {
            SystemManager?.StartSystem(typeof(ExampleStartupEntityFrameworkCore));
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