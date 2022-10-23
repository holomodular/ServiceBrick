using ServiceBrick;
using System.Reflection;

namespace Example.AzureDataTables
{
    public class ExampleAzureDataTablesModule : IModule
    {
        public ExampleAzureDataTablesModule()
        {
            AdminHtml = string.Empty;
            Name = "Example AzureDataTables Brick";
            Description = @"The Example AzureDataTables Brick implements the Azure Data Tables provider.";
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(ExampleAzureDataTablesModule).Assembly
            };
        }

        public string Name { get; }
        public string Description { get; }
        public string AdminHtml { get; }
        public List<Assembly> AutomapperAssemblies { get; }
        public List<Assembly> ViewAssemblies { get; }
    }
}