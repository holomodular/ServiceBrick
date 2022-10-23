using ServiceBrick.Xunit;

namespace Example.AzureDataTables.Xunit
{
    [Collection("AzureDataTables")]
    public class MappingTest : ServiceBrick.Xunit.MappingTest, IDisposable
    {
        public MappingTest()
        {
            SystemManager = new SystemManager();
            SystemManager?.StartSystem(typeof(ExampleStartupAzureDataTables));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}