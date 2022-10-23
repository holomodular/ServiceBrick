namespace Example.MongoDb.Xunit
{
    [Collection("MongoDb")]
    public class CategoryApiControllerTest : Example.Xunit.CategoryApiControllerTest, IDisposable
    {
        public CategoryApiControllerTest() : base()
        {
            TestData = new CategoryTestData();
            SystemManager?.StartSystem(typeof(ExampleStartupMongoDb));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}