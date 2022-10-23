using ServiceBrick.Xunit;

namespace Example.EntityFrameworkCore.Xunit.InMemory
{
    [Collection("EntityFrameworkCore")]
    public class MappingTest : ServiceBrick.Xunit.MappingTest, IDisposable
    {
        public MappingTest()
        {
            SystemManager = new SystemManager();
            SystemManager?.StartSystem(typeof(ExampleStartupEntityFrameworkCoreInMemory));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}