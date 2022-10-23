using ServiceBrick.Xunit;

namespace Example.MongoDb.Xunit
{
    [Collection("MongoDb")]
    public class MappingTest : ServiceBrick.Xunit.MappingTest, IDisposable
    {
        public MappingTest()
        {
            SystemManager = new SystemManager();
            SystemManager?.StartSystem(typeof(ExampleStartupMongoDb));
        }

        public virtual void Dispose()
        {
            SystemManager?.StopSystem();
        }
    }
}