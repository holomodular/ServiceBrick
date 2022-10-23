using ServiceBrick;
using System.Reflection;

namespace Example.EntityFrameworkCore
{
    public class ExampleEntityFrameworkCoreModule : IModule
    {
        public ExampleEntityFrameworkCoreModule()
        {
            AdminHtml = string.Empty;
            Name = "Example EntityFrameworkCore Brick";
            Description = @"The Example EntityFrameworkCore Brick implements the Entity Framework Core provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(ExampleEntityFrameworkCoreModule).Assembly
            };
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}