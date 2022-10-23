using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBrick.Storage.AzureDataTables;

namespace Example.AzureDataTables
{
    public class ExampleStorageRepository<TDomain> : AzureDataTablesStorageRepository<TDomain>
        where TDomain : class, IAzureDataTablesDomainObject<TDomain>, new()
    {
        public ExampleStorageRepository(
            ILoggerFactory logFactory,
            IConfiguration configuration)
            : base(logFactory)
        {
            ConnectionString = configuration.GetAzureDataTablesConnectionString(
                ExampleAzureDataTablesConstants.APPSETTINGS_CONNECTION_STRING);
            TableName = ExampleAzureDataTablesConstants.GetTableName(typeof(TDomain).Name);
        }
    }
}