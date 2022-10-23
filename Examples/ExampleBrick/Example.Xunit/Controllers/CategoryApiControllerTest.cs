using Example.Api;
using Microsoft.AspNetCore.Mvc;
using ServiceBrick;
using ServiceBrick.Xunit;

namespace Example.Xunit
{
    public abstract class CategoryApiControllerTest : ApiControllerTest<CategoryDto>
    {
        public CategoryApiControllerTest()
        {
            SystemManager = new SystemManager();
            TestData = new CategoryTestData();
        }

        //Additional tests for business rules

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
            var controller = TestData.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.Update(dto);
            if (respUpdate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is CategoryDto obj)
                {
                    // Create date should not change
                    Assert.True(obj.CreateDate == startingCreateDate);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
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
            var controller = TestData.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.Update(dto);
            if (respUpdate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Change the UpdateDate property back
            dto.UpdateDate = existingUpdate;

            //Call Update
            respUpdate = await controller.Update(dto);
            if (respUpdate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is CategoryDto obj)
                    Assert.True(obj.UpdateDate > existingUpdate);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }
    }
}