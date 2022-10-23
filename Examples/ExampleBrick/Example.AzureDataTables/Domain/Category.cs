using ServiceBrick;
using ServiceBrick.Storage.AzureDataTables;

namespace Example.AzureDataTables
{
    public partial class Category : AzureDataTablesDomainObject<Category>, IDpCreateDate, IDpUpdateDate
    {
        public Category()
        {
        }

        public virtual string Name { get; set; }
        public virtual DateTimeOffset CreateDate { get; set; }
        public virtual DateTimeOffset UpdateDate { get; set; }
    }
}