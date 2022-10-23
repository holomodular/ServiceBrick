using Example.Api;
using ServiceBrick.Xunit;

namespace Example.Xunit
{
    public abstract class CategoryApiClientTest : ApiClientTest<CategoryDto>
    {
        public CategoryApiClientTest()
        {
            SystemManager = new SystemManager();
            TestData = new CategoryTestData();
        }

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
    }
}