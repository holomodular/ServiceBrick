namespace Example.MongoDb
{
    public static class ExampleMongoDbConstants
    {
        public const string APPSETTINGS_CONNECTION_STRING = "ServiceBrick:Example:MongoDb:ConnectionString";
        public const string APPSETTINGS_DATABASE_NAME = "ServiceBrick:Example:MongoDb:DatabaseName";

        public const string COLLECTIONNAME_PREFIX = "Example";

        public static string GetCollectionName(string tableName)
        {
            return COLLECTIONNAME_PREFIX + tableName;
        }
    }
}