using ServiceBrick;
using System.Reflection;

namespace Example.Api.Controller
{
    public class ExampleApiControllerModule : IModule
    {
        public ExampleApiControllerModule()
        {
            AdminHtml = string.Empty;
            Name = "Example API Controller Brick";
            Description = @"The Example API Controller Brick contains controllers that expose the API services.";
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}