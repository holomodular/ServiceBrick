namespace Example.AzureDataTables
{
    public static class ExampleAzureDataTablesConstants
    {
        public const string APPSETTINGS_CONNECTION_STRING = "ServiceBrick:Example:AzureDataTables:ConnectionString";

        public const string TABLENAME_PREFIX = "Example";

        public static string GetTableName(string tableName)
        {
            return TABLENAME_PREFIX + tableName;
        }
    }
}