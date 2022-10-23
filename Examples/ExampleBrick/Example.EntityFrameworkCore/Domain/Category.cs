using ServiceBrick;
using ServiceBrick.Storage.EntityFrameworkCore;

namespace Example.EntityFrameworkCore
{
    public partial class Category : EntityFrameworkCoreDomainObject<Category>, IDpCreateDate, IDpUpdateDate
    {
        public Category()
        {
            ProductCategories = new List<ProductCategory>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        public override IQueryable<Category> DomainGetItemQueryable(IQueryable<Category> query, Category obj)
        {
            return query.Where(x => x.ID == obj.ID);
        }
    }
}