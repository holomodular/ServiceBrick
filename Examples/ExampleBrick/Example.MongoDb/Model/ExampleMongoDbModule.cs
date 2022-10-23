using ServiceBrick;
using System.Reflection;

namespace Example.MongoDb
{
    public class ExampleMongoDbModule : IModule
    {
        public ExampleMongoDbModule()
        {
            AdminHtml = string.Empty;
            Name = "Example MongoDb Brick";
            Description = @"The Example MongoDB Brick implements the MongoDB provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(ExampleMongoDbModule).Assembly
            };
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}