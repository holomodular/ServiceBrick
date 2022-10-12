using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBrick;
using ServiceBrick.Logging.Api;
using ServiceBrick.Cache.Api;
using ServiceBrick.Notification.Api;
using ServiceBrick.Security.Api;
using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace Client
{
    public class ServiceBrickCacheExample
    {
        public void ShowExample()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var dataApiClient = Program.ServiceProvider.GetRequiredService<IDataApiClient>();

                // Create cache data
                DataDto data = new DataDto()
                {
                    Key = "MyUniqueKeyName",
                    Value = "SomeValue"
                };
                var respCreate = dataApiClient.CreateAsync(data).GetAwaiter().GetResult();
                if (respCreate.Error)
                    throw new Exception(respCreate.ToString());

                // Update cache data
                data = respCreate.Item;
                data.Value = "NewValue";
                var respUpdate = dataApiClient.UpdateAsync(data).GetAwaiter().GetResult();
                if (respUpdate.Error)
                    throw new Exception(respUpdate.ToString());

                // Get cache data by its storagekey (primary key)
                var respGet = dataApiClient.GetItemAsync(data.StorageKey).GetAwaiter().GetResult();
                if (respGet.Error)
                    throw new Exception(respGet.ToString());
                if (respGet.Item == null)
                    throw new Exception("GetItem not found");

                // Get cache data by a list of storagekeys (primary keys)
                var respGetItems = dataApiClient.GetItemsAsync(
                    new List<string>() { data.StorageKey }).GetAwaiter().GetResult();
                if (respGetItems.Error)
                    throw new Exception(respGet.ToString());

                // Get all cache data
                var respGetAll = dataApiClient.GetAllAsync().GetAwaiter().GetResult();
                if (respGetAll.Error)
                    throw new Exception(respGetAll.ToString());
                if (respGetAll.List == null || respGetAll.List.Count == 0)
                    throw new Exception("GetAll no items found");

                // Query for log messages by its Key
                var query = QueryBuilder
                    .New()
                    .IsEqual(nameof(DataDto.Key), "MyUniqueKeyName")
                    .Build();
                var respQuery = dataApiClient.QueryAsync(query).GetAwaiter().GetResult();
                if (respQuery.Error)
                    throw new Exception(respQuery.ToString());
                if (respQuery.List == null || respQuery.List.Count == 0)
                    throw new Exception("Query no items found");

                // Delete a log message by its StorageKey (primary key)
                var respDelete = dataApiClient.DeleteAsync(data.StorageKey).GetAwaiter().GetResult();
                if (respDelete.Error)
                    throw new Exception(respDelete.ToString());
            }
        }
    }
}