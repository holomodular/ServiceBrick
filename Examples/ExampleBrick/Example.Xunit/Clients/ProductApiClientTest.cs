using Example.Api;
using ServiceBrick.Xunit;

namespace Example.Xunit
{
    public abstract class ProductApiClientTest : ApiClientTest<ProductDto>
    {
        public ProductApiClientTest()
        {
            SystemManager = new SystemManager();
            TestData = new ProductTestData();
        }

        public abstract ApiClientTest<CategoryDto> GetCategoryClient();

        //Additional tests for business rules

        [Fact]
        public virtual async Task Create_NameRequired()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var model = TestData.GetMinimumDataObject();
            model.Name = String.Empty;

            //Call Create
            var client = TestData.GetClient(SystemManager.ServiceProvider);
            var respCreate = await client.CreateAsync(model);
            Assert.True(respCreate.Error == true);
        }

        [Fact]
        public virtual async Task Update_NameRequired()
        {
            var model = TestData.GetMinimumDataObject();
            var dto = await CreateBase(model);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            //Update the Name property
            dto.Name = string.Empty;

            //Call Update
            var client = TestData.GetClient(SystemManager.ServiceProvider);
            var respUpdate = await client.UpdateAsync(dto);
            Assert.True(respUpdate.Error == true);
        }

        [Fact]
        public virtual async Task Update_CreateDate()
        {
            var model = TestData.GetMinimumDataObject();
            var dto = await CreateBase(model);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            DateTimeOffset startingCreateDate = dto.CreateDate;

            //Update the CreateDate property
            dto.CreateDate = DateTimeOffset.UtcNow;

            //Call Update
            var client = TestData.GetClient(SystemManager.ServiceProvider);
            var respUpdate = await client.UpdateAsync(dto);
            Assert.True(respUpdate.Success == true);
            Assert.True(respUpdate.Item.CreateDate == startingCreateDate);
        }

        [Fact]
        public virtual async Task Update_UpdateDateConcurrency()
        {
            var model = TestData.GetMinimumDataObject();
            var dto = await CreateBase(model);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var existingUpdate = dto.UpdateDate;

            //Change the UpdateDate property
            dto.UpdateDate = DateTimeOffset.UtcNow;

            //Call Update
            var client = TestData.GetClient(SystemManager.ServiceProvider);
            var respUpdate = await client.UpdateAsync(dto);
            Assert.True(respUpdate.Error == true);

            //Change the UpdateDate property back
            dto.UpdateDate = existingUpdate;

            //Call Update
            respUpdate = await client.UpdateAsync(dto);
            Assert.True(respUpdate.Success == true);
            Assert.True(respUpdate.Item.UpdateDate > existingUpdate);
        }

        [Fact]
        public virtual async Task Update_ExpirationDate()
        {
            var model = TestData.GetMinimumDataObject();
            var dto = await CreateBase(model);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var existingExpiration = dto.ExpirationDate;

            //Change the property to different offset
            dto.ExpirationDate = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(6));

            //Call Update
            var client = TestData.GetClient(SystemManager.ServiceProvider);
            var respUpdate = await client.UpdateAsync(dto);
            Assert.True(respUpdate.Success == true);
            Assert.True(respUpdate.Item.ExpirationDate == dto.ExpirationDate);
            Assert.True(respUpdate.Item.ExpirationDate.Offset == TimeSpan.Zero);
        }

        protected virtual CategoryDto CreateCategoryDependency()
        {
            var catClient = GetCategoryClient();
            catClient.SystemManager = this.SystemManager;
            var category = catClient.TestData.GetMinimumDataObject();
            return catClient.CreateBase(category).Result;
        }

        [Fact]
        public virtual async Task AddCategory()
        {
            var model = TestData.GetMinimumDataObject();
            var dto = await CreateBase(model);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            // Create category dependency
            var category = CreateCategoryDependency();
            if (category == null)
                throw new Exception("Category dependency is null");

            var client = TestData.GetClient(SystemManager.ServiceProvider);
            IProductApiClient productApiClient = (IProductApiClient)client;

            //Call Add
            var respUpdate = await productApiClient.AddCategoryAsync(dto.StorageKey, category.StorageKey);
            Assert.True(respUpdate.Success == true);

            // Get item
            var respGet = await client.GetItemAsync(dto.StorageKey);
            Assert.True(respGet.Success == true);
            Assert.True(respGet.Item.Categories != null);
            Assert.True(respGet.Item.Categories.Count == 1);
            Assert.True(respGet.Item.Categories[0].StorageKey == category.StorageKey);
            Assert.True(respGet.Item.Categories[0].Name == category.Name);
        }

        [Fact]
        public virtual async Task AddCategory_Duplicate()
        {
            var model = TestData.GetMinimumDataObject();
            var dto = await CreateBase(model);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            // Create category dependency
            var category = CreateCategoryDependency();
            if (category == null)
                throw new Exception("Category dependency is null");

            var client = TestData.GetClient(SystemManager.ServiceProvider);
            IProductApiClient productApiClient = (IProductApiClient)client;

            //Call Add
            var respAdd = await productApiClient.AddCategoryAsync(dto.StorageKey, category.StorageKey);
            Assert.True(respAdd.Success == true);

            //Call Add again
            var respAdd2 = await productApiClient.AddCategoryAsync(dto.StorageKey, category.StorageKey);
            Assert.True(respAdd2.Error == true);
        }

        [Fact]
        public virtual async Task RemoveCategory()
        {
            var model = TestData.GetMinimumDataObject();
            var dto = await CreateBase(model);

            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            // Create category dependency
            var category = CreateCategoryDependency();
            if (category == null)
                throw new Exception("Category dependency is null");

            var client = TestData.GetClient(SystemManager.ServiceProvider);
            IProductApiClient productApiClient = (IProductApiClient)client;

            //Call Add
            var respAdd = await productApiClient.AddCategoryAsync(dto.StorageKey, category.StorageKey);
            Assert.True(respAdd.Success == true);

            //Call Remove
            var respRemove = await productApiClient.RemoveCategoryAsync(dto.StorageKey, category.StorageKey);
            Assert.True(respRemove.Success == true);

            // Get item
            var respGet = await client.GetItemAsync(dto.StorageKey);
            Assert.True(respGet.Success == true);
            Assert.True(respGet.Item.Categories != null);
            Assert.True(respGet.Item.Categories.Count == 0);
        }
    }
}