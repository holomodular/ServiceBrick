using Example.Api;
using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;

namespace Client
{
    public class ServiceBrickExampleProduct
    {
        public void ShowExample()
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var productApiClient = Program.ServiceProvider.GetRequiredService<IProductApiClient>();

                // Create Product
                ProductDto product = new ProductDto()
                {
                    Name = "Food"
                };
                var respCreate = productApiClient.CreateAsync(product).GetAwaiter().GetResult();
                if (respCreate.Error)
                    throw new Exception(respCreate.ToString());

                // Update Product
                product = respCreate.Item;
                product.Name = "Cheddar";
                var respUpdate = productApiClient.UpdateAsync(product).GetAwaiter().GetResult();
                if (respUpdate.Error)
                    throw new Exception(respUpdate.ToString());

                // Get Product by its storagekey (primary key)
                var respGet = productApiClient.GetItemAsync(product.StorageKey).GetAwaiter().GetResult();
                if (respGet.Error)
                    throw new Exception(respGet.ToString());
                if (respGet.Item == null)
                    throw new Exception("GetItem not found");

                // Get Product data by a list of storagekeys (primary keys)
                var respGetItems = productApiClient.GetItemsAsync(
                    new List<string>() { product.StorageKey }).GetAwaiter().GetResult();
                if (respGetItems.Error)
                    throw new Exception(respGet.ToString());

                // Get all Product data
                var respGetAll = productApiClient.GetAllAsync().GetAwaiter().GetResult();
                if (respGetAll.Error)
                    throw new Exception(respGetAll.ToString());
                if (respGetAll.List == null || respGetAll.List.Count == 0)
                    throw new Exception("GetAll no items found");

                // Query for Product by its Key
                var query = QueryBuilder
                    .New()
                    .IsEqual(nameof(ProductDto.Name), "Cheddar")
                    .Build();
                var respQuery = productApiClient.QueryAsync(query).GetAwaiter().GetResult();
                if (respQuery.Error)
                    throw new Exception(respQuery.ToString());
                if (respQuery.List == null || respQuery.List.Count == 0)
                    throw new Exception("Query no items found");

                // Create a category
                var categoryApiClient = Program.ServiceProvider.GetRequiredService<ICategoryApiClient>();
                var category = new CategoryDto()
                {
                    Name = "Cheese"
                };
                var respCreateCategory = categoryApiClient.CreateAsync(category).GetAwaiter().GetResult();
                category = respCreateCategory.Item;

                // Add category to product
                var respAddCat = productApiClient.AddCategoryAsync(product.StorageKey, category.StorageKey).GetAwaiter().GetResult();
                if (respAddCat.Error)
                    throw new Exception("Did not add category");

                // Get product again
                respGet = productApiClient.GetItemAsync(product.StorageKey).GetAwaiter().GetResult();
                var catFound = respGet.Item.Categories.Where(x => x.StorageKey == category.StorageKey).FirstOrDefault();
                if (catFound == null)
                    throw new Exception("Category not linked");

                // Remove category from product
                var respRemoveCat = productApiClient.RemoveCategoryAsync(product.StorageKey, category.StorageKey).GetAwaiter().GetResult();
                if (respRemoveCat.Error)
                    throw new Exception("Did not remove category");

                // Delete a product by its StorageKey (primary key)
                var respDelete = productApiClient.DeleteAsync(product.StorageKey).GetAwaiter().GetResult();
                if (respDelete.Error)
                    throw new Exception(respDelete.ToString());
            }
        }
    }
}