using ServiceBrick;
using ServiceBrick.Storage.AzureDataTables;

namespace Example.AzureDataTables
{
    public partial class Product : AzureDataTablesDomainObject<Product>, IDpCreateDate, IDpUpdateDate
    {
        public Product()
        {
        }

        public virtual string Name { get; set; }
        public virtual DateTimeOffset CreateDate { get; set; }
        public virtual DateTimeOffset UpdateDate { get; set; }
        public bool IsExpired { get; set; }
        public virtual DateTimeOffset? ExpirationDate { get; set; }
    }
}