using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBrick.Storage.MongoDb;

namespace Example.MongoDb
{
    public class ExampleStorageRepository<TDomain> : MongoDbStorageRepository<TDomain>
        where TDomain : class, IMongoDbDomainObject<TDomain>, new()
    {
        public ExampleStorageRepository(
            ILoggerFactory logFactory,
            IConfiguration configuration)
            : base(logFactory)
        {
            ConnectionString = configuration.GetMongoDbConnectionString(
                ExampleMongoDbConstants.APPSETTINGS_CONNECTION_STRING);
            DatabaseName = configuration.GetMongoDbDatabaseName(
                ExampleMongoDbConstants.APPSETTINGS_DATABASE_NAME);
            CollectionName = ExampleMongoDbConstants.GetCollectionName(typeof(TDomain).Name);
        }
    }
}