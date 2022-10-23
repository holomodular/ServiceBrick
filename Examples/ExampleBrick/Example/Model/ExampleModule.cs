using ServiceBrick;
using System.Reflection;

namespace Example
{
    public class ExampleModule : IModule
    {
        public ExampleModule()
        {
            AdminHtml = string.Empty;
            Name = "Example Brick";
            Description = @"The Example Brick is an example that links products to categories.";
            ViewAssemblies = new List<Assembly>();
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}