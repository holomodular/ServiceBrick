using ServiceBrick.Xunit;

namespace Example.EntityFrameworkCore.Xunit
{
    [Collection("EntityFrameworkCore")]
    public class MappingTest : ServiceBrick.Xunit.MappingTest, IDisposable
    {
        public MappingTest()
        {
            SystemManager = new SystemManager();
            SystemManager?.StartSystem(typeof(ExampleStartupEntityFrameworkCore));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}