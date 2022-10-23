namespace Example.EntityFrameworkCore.Xunit
{
    [Collection("EntityFrameworkCore")]
    public class CategoryApiControllerTest : Example.Xunit.CategoryApiControllerTest, IDisposable
    {
        public CategoryApiControllerTest() : base()
        {
            SystemManager?.StartSystem(typeof(ExampleStartupEntityFrameworkCore));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}