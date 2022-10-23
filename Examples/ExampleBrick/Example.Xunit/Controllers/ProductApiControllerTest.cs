using Example.Api;
using Microsoft.AspNetCore.Mvc;
using ServiceBrick;
using ServiceBrick.Xunit;

namespace Example.Xunit
{
    public abstract class ProductApiControllerTest : ApiControllerTest<ProductDto>
    {
        public ProductApiControllerTest()
        {
            SystemManager = new SystemManager();
            TestData = new ProductTestData();
        }

        public abstract ApiControllerTest<CategoryDto> GetCategoryController();

        //Additional tests for business rules

        [Fact]
        public virtual async Task Create_NameRequired()
        {
            if (SystemManager == null || SystemManager.ServiceProvider == null)
                throw new ArgumentNullException(nameof(SystemManager));

            var model = TestData.GetMinimumDataObject();
            model.Name = String.Empty;

            //Call Create
            var controller = TestData.GetController(SystemManager.ServiceProvider);
            var respCreate = await controller.Create(model);
            if (respCreate is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
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
            var controller = TestData.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.Update(dto);
            if (respUpdate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ProductDto obj)
                {
                    // CreateDate should not change
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
                if (okResult.Value is ProductDto obj)
                    Assert.True(obj.UpdateDate > existingUpdate);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
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
            var controller = TestData.GetController(SystemManager.ServiceProvider);
            var respUpdate = await controller.Update(dto);
            if (respUpdate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is ProductDto obj)
                {
                    Assert.True(obj.ExpirationDate == dto.ExpirationDate);
                    Assert.True(obj.ExpirationDate.Offset == TimeSpan.Zero);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        protected virtual CategoryDto CreateCategoryDependency()
        {
            var catController = GetCategoryController();
            catController.SystemManager = this.SystemManager;
            var category = catController.TestData.GetMinimumDataObject();
            return catController.CreateBase(category).Result;
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

            var controller = TestData.GetController(SystemManager.ServiceProvider);
            IProductApiController productApiController = (IProductApiController)controller;

            //Call Add
            var respUpdate = await productApiController.AddCategory(dto.StorageKey, category.StorageKey);
            if (respUpdate is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is bool boolValue)
                    Assert.True(boolValue == true);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Get item
            var respGet = await controller.GetItem(dto.StorageKey);
            if (respGet is OkObjectResult okGetResult)
            {
                Assert.True(okGetResult.Value != null);
                if (okGetResult.Value is ProductDto getProduct)
                {
                    Assert.True(getProduct.Categories != null);
                    Assert.True(getProduct.Categories.Count == 1);
                    Assert.True(getProduct.Categories[0].StorageKey == category.StorageKey);
                    Assert.True(getProduct.Categories[0].Name == category.Name);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
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

            var controller = TestData.GetController(SystemManager.ServiceProvider);
            IProductApiController productApiController = (IProductApiController)controller;

            //Call Add
            var respAdd = await productApiController.AddCategory(dto.StorageKey, category.StorageKey);
            if (respAdd is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is bool boolValue)
                    Assert.True(boolValue == true);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call Add again
            var respAdd2 = await productApiController.AddCategory(dto.StorageKey, category.StorageKey);
            if (respAdd2 is BadRequestObjectResult badResult)
            {
                Assert.True(badResult.Value != null);
                if (badResult.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
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

            var controller = TestData.GetController(SystemManager.ServiceProvider);
            IProductApiController productApiController = (IProductApiController)controller;

            //Call Add
            var respAdd = await productApiController.AddCategory(dto.StorageKey, category.StorageKey);
            if (respAdd is OkObjectResult okResult)
            {
                Assert.True(okResult.Value != null);
                if (okResult.Value is bool boolValue)
                    Assert.True(boolValue == true);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Call Remove
            var respRemove = await productApiController.RemoveCategory(dto.StorageKey, category.StorageKey);
            if (respRemove is OkObjectResult okRemoveResult)
            {
                Assert.True(okRemoveResult.Value != null);
                if (okRemoveResult.Value is bool boolValue)
                    Assert.True(boolValue == true);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Get item
            var respGet = await controller.GetItem(dto.StorageKey);
            if (respGet is OkObjectResult okGetResult)
            {
                Assert.True(okGetResult.Value != null);
                if (okGetResult.Value is ProductDto getProduct)
                {
                    Assert.True(getProduct.Categories != null);
                    Assert.True(getProduct.Categories.Count == 0);
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }
    }
}