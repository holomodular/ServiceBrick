using Example.Api;

namespace Example.MongoDb.Xunit
{
    public class ProductTestData : Example.Xunit.ProductTestData
    {
        public override ProductDto GetObjectNotFound()
        {
            return new ProductDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }
}