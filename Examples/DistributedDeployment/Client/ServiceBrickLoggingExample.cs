using Microsoft.Extensions.DependencyInjection;
using ServiceBrick.Logging.Api;
using ServiceQuery;

namespace Client
{
    public class ServiceBrickLoggingExample
    {
        public void ShowExample()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var logApiClient = Program.ServiceProvider.GetRequiredService<ILogMessageApiClient>();

                // Create a log message
                LogMessageDto logMessage = new LogMessageDto()
                {
                    Application = "MyApp",
                    Level = "Information",
                    Message = "Hello World"
                };
                var respCreate = logApiClient.CreateAsync(logMessage).GetAwaiter().GetResult();
                if (respCreate.Error)
                    throw new Exception(respCreate.ToString());

                // Update a log message
                logMessage = respCreate.Item;
                logMessage.Category = "Update Category";
                logMessage.Server = "Test Server";
                var respUpdate = logApiClient.UpdateAsync(logMessage).GetAwaiter().GetResult();
                if (respUpdate.Error)
                    throw new Exception(respUpdate.ToString());

                // Get a log message by its storagekey (primary key)
                var respGet = logApiClient.GetItemAsync(logMessage.StorageKey).GetAwaiter().GetResult();
                if (respGet.Error)
                    throw new Exception(respGet.ToString());
                if (respGet.Item == null)
                    throw new Exception("GetItem not found");

                // Get log messages by a list of storagekeys (primary keys)
                var respGetItems = logApiClient.GetItemsAsync(
                    new List<string>() { logMessage.StorageKey }).GetAwaiter().GetResult();
                if (respGetItems.Error)
                    throw new Exception(respGet.ToString());

                // Get all log messages
                var respGetAll = logApiClient.GetAllAsync().GetAwaiter().GetResult();
                if (respGetAll.Error)
                    throw new Exception(respGetAll.ToString());
                if (respGetAll.List == null || respGetAll.List.Count == 0)
                    throw new Exception("GetAll no items found");

                // Query for log messages by its Message and Server
                var query = QueryBuilder
                    .New()
                    .IsEqual(nameof(LogMessageDto.Message), "Hello World")
                    .And()
                    .IsEqual(nameof(LogMessageDto.Server), "Test Server")
                    .Build();
                var respQuery = logApiClient.QueryAsync(query).GetAwaiter().GetResult();
                if (respQuery.Error)
                    throw new Exception(respQuery.ToString());
                if (respQuery.List == null || respQuery.List.Count == 0)
                    throw new Exception("Query no items found");

                // Delete a log message by its StorageKey (primary key)
                var respDelete = logApiClient.DeleteAsync(logMessage.StorageKey).GetAwaiter().GetResult();
                if (respDelete.Error)
                    throw new Exception(respDelete.ToString());
            }
        }
    }
}