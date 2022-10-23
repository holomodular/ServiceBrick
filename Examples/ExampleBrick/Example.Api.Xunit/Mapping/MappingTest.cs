using ServiceBrick.Xunit;

namespace Example.Api.Xunit
{
    [Collection("Api")]
    public class MappingTest : ServiceBrick.Xunit.MappingTest, IDisposable
    {
        public MappingTest()
        {
            SystemManager = new SystemManager();
            SystemManager?.StartSystem(typeof(ExampleStartupApi));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}