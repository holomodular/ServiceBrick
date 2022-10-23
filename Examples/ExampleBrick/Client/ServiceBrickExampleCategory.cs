using Example.Api;
using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace Client
{
    public class ServiceBrickExampleCategory
    {
        public void ShowExample()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var categoryApiClient = Program.ServiceProvider.GetRequiredService<ICategoryApiClient>();

                // Create category
                CategoryDto category = new CategoryDto()
                {
                    Name = "Food"
                };
                var respCreate = categoryApiClient.CreateAsync(category).GetAwaiter().GetResult();
                if (respCreate.Error)
                    throw new Exception(respCreate.ToString());

                // Update category
                category = respCreate.Item;
                category.Name = "Dairy";
                var respUpdate = categoryApiClient.UpdateAsync(category).GetAwaiter().GetResult();
                if (respUpdate.Error)
                    throw new Exception(respUpdate.ToString());

                // Get category by its storagekey (primary key)
                var respGet = categoryApiClient.GetItemAsync(category.StorageKey).GetAwaiter().GetResult();
                if (respGet.Error)
                    throw new Exception(respGet.ToString());
                if (respGet.Item == null)
                    throw new Exception("GetItem not found");

                // Get category data by a list of storagekeys (primary keys)
                var respGetItems = categoryApiClient.GetItemsAsync(
                    new List<string>() { category.StorageKey }).GetAwaiter().GetResult();
                if (respGetItems.Error)
                    throw new Exception(respGet.ToString());

                // Get all category data
                var respGetAll = categoryApiClient.GetAllAsync().GetAwaiter().GetResult();
                if (respGetAll.Error)
                    throw new Exception(respGetAll.ToString());
                if (respGetAll.List == null || respGetAll.List.Count == 0)
                    throw new Exception("GetAll no items found");

                // Query for category by its Key
                var query = QueryBuilder
                    .New()
                    .IsEqual(nameof(CategoryDto.Name), "Dairy")
                    .Build();
                var respQuery = categoryApiClient.QueryAsync(query).GetAwaiter().GetResult();
                if (respQuery.Error)
                    throw new Exception(respQuery.ToString());
                if (respQuery.List == null || respQuery.List.Count == 0)
                    throw new Exception("Query no items found");

                // Delete a log message by its StorageKey (primary key)
                var respDelete = categoryApiClient.DeleteAsync(category.StorageKey).GetAwaiter().GetResult();
                if (respDelete.Error)
                    throw new Exception(respDelete.ToString());
            }
        }
    }
}