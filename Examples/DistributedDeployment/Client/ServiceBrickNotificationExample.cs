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
    public class ServiceBrickNotificationExample
    {
        public void ShowExample()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var messageApiClient = Program.ServiceProvider.GetRequiredService<IMessageApiClient>();

                // Create a notification message
                MessageDto message = new MessageDto()
                {
                    ToAddress = "test@servicebrick.com",
                    Subject = "Test Message",
                    Body = "This is a test message.",
                    SenderTypeKey = 1
                };
                var respCreate = messageApiClient.CreateAsync(message).GetAwaiter().GetResult();
                if (respCreate.Error)
                    throw new Exception(respCreate.ToString());

                // Update a notification message
                message = respCreate.Item;
                message.CcAddress = "test2@servicebrick.com";
                var respUpdate = messageApiClient.UpdateAsync(message).GetAwaiter().GetResult();
                if (respUpdate.Error)
                    throw new Exception(respUpdate.ToString());

                // Get a notification message by its storagekey (primary key)
                var respGet = messageApiClient.GetItemAsync(message.StorageKey).GetAwaiter().GetResult();
                if (respGet.Error)
                    throw new Exception(respGet.ToString());
                if (respGet.Item == null)
                    throw new Exception("GetItem not found");

                // Get cache data by a list of storagekeys (primary keys)
                var respGetItems = messageApiClient.GetItemsAsync(
                    new List<string>() { message.StorageKey }).GetAwaiter().GetResult();
                if (respGetItems.Error)
                    throw new Exception(respGet.ToString());

                // Get all notification messages
                var respGetAll = messageApiClient.GetAllAsync().GetAwaiter().GetResult();
                if (respGetAll.Error)
                    throw new Exception(respGetAll.ToString());
                if (respGetAll.List == null || respGetAll.List.Count == 0)
                    throw new Exception("GetAll no items found");

                // Query for log messages by its ToAddress and CcAddress
                var query = QueryBuilder
                    .New()
                    .IsEqual(nameof(MessageDto.ToAddress), "test@servicebrick.com")
                    .And()
                    .IsEqual(nameof(MessageDto.CcAddress), "test2@servicebrick.com")
                    .Build();
                var respQuery = messageApiClient.QueryAsync(query).GetAwaiter().GetResult();
                if (respQuery.Error)
                    throw new Exception(respQuery.ToString());
                if (respQuery.List == null || respQuery.List.Count == 0)
                    throw new Exception("Query no items found");

                // Delete a log message by its StorageKey (primary key)
                var respDelete = messageApiClient.DeleteAsync(message.StorageKey).GetAwaiter().GetResult();
                if (respDelete.Error)
                    throw new Exception(respDelete.ToString());
            }
        }
    }
}