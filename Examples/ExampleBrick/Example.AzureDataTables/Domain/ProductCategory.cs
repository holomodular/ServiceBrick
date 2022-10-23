using ServiceBrick.Storage.AzureDataTables;

namespace Example.AzureDataTables
{
    public partial class ProductCategory : AzureDataTablesDomainObject<ProductCategory>
    {
        public ProductCategory()
        {
        }

        public virtual string CategoryPartitionKey { get; set; }
        public virtual string ProductPartitionKey { get; set; }
    }
}