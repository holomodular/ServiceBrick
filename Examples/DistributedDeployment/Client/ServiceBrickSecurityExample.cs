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
    public class ServiceBrickSecurityExample
    {
        public void ShowExample()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var applicationUserApiClient = Program.ServiceProvider.GetRequiredService<IApplicationUserApiClient>();

                // Create a user
                string emailAddress = Guid.NewGuid().ToString() + "@servicebrick.com";
                ApplicationUserDto user = new ApplicationUserDto()
                {
                    Email = emailAddress,
                    UserName = emailAddress
                };
                var respCreate = applicationUserApiClient.CreateAsync(user).GetAwaiter().GetResult();
                if (respCreate.Error)
                    throw new Exception(respCreate.ToString());

                // Update a user
                user = respCreate.Item;
                user.PhoneNumber = "123456789";
                var respUpdate = applicationUserApiClient.UpdateAsync(user).GetAwaiter().GetResult();
                if (respUpdate.Error)
                    throw new Exception(respUpdate.ToString());

                // Get a user by its storagekey (primary key)
                var respGet = applicationUserApiClient.GetItemAsync(user.StorageKey).GetAwaiter().GetResult();
                if (respGet.Error)
                    throw new Exception(respGet.ToString());
                if (respGet.Item == null)
                    throw new Exception("GetItem not found");

                // Get users by a list of storagekeys (primary keys)
                var respGetItems = applicationUserApiClient.GetItemsAsync(
                    new List<string>() { user.StorageKey }).GetAwaiter().GetResult();
                if (respGetItems.Error)
                    throw new Exception(respGet.ToString());

                // Get all users
                var respGetAll = applicationUserApiClient.GetAllAsync().GetAwaiter().GetResult();
                if (respGetAll.Error)
                    throw new Exception(respGetAll.ToString());
                if (respGetAll.List == null || respGetAll.List.Count == 0)
                    throw new Exception("GetAll no items found");

                // Query for log messages by its Email and PhoneNumber
                var query = QueryBuilder
                    .New()
                    .IsEqual(nameof(ApplicationUserDto.Email), emailAddress)
                    .And()
                    .IsEqual(nameof(ApplicationUserDto.PhoneNumber), "123456789")
                    .Build();
                var respQuery = applicationUserApiClient.QueryAsync(query).GetAwaiter().GetResult();
                if (respQuery.Error)
                    throw new Exception(respQuery.ToString());
                if (respQuery.List == null || respQuery.List.Count == 0)
                    throw new Exception("Query no items found");

                // Delete a log message by its StorageKey (primary key)
                var respDelete = applicationUserApiClient.DeleteAsync(user.StorageKey).GetAwaiter().GetResult();
                if (respDelete.Error)
                    throw new Exception(respDelete.ToString());
            }
        }
    }
}