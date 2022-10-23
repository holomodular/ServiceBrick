namespace Example.AzureDataTables.Xunit
{
    [Collection("AzureDataTables")]
    public class CategoryApiControllerTest : Example.Xunit.CategoryApiControllerTest, IDisposable
    {
        public CategoryApiControllerTest() : base()
        {
            SystemManager?.StartSystem(typeof(ExampleStartupAzureDataTables));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}