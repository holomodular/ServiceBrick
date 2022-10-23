namespace Example.EntityFrameworkCore.Xunit.InMemory
{
    [Collection("EntityFrameworkCore")]
    public class CategoryApiControllerTest : Example.Xunit.CategoryApiControllerTest, IDisposable
    {
        public CategoryApiControllerTest() : base()
        {
            SystemManager?.StartSystem(typeof(ExampleStartupEntityFrameworkCoreInMemory));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}