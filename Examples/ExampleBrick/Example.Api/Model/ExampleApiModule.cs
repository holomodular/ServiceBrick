using ServiceBrick;
using System.Reflection;

namespace Example.Api
{
    public class ExampleApiModule : IModule
    {
        public ExampleApiModule()
        {
            AdminHtml = string.Empty;
            Name = "Example API Brick";
            Description = @"The Example API Brick contains clients, data transfer objects, events and interfaces.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}