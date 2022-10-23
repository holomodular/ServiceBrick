using Example.Api;

namespace Example.MongoDb.Xunit
{
    public class CategoryTestData : Example.Xunit.CategoryTestData
    {
        public override CategoryDto GetObjectNotFound()
        {
            return new CategoryDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }
}