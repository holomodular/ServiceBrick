using Microsoft.EntityFrameworkCore;
using ServiceBrick;
using ServiceBrick.Storage.EntityFrameworkCore;

namespace Example.EntityFrameworkCore
{
    public partial class Product : EntityFrameworkCoreDomainObject<Product>, IDpCreateDate, IDpUpdateDate
    {
        public Product()
        {
            ProductCategories = new List<ProductCategory>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public bool IsExpired { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        public override IQueryable<Product> DomainGetItemQueryable(IQueryable<Product> query, Product obj)
        {
            return query.Where(x => x.ID == obj.ID);
        }

        public override IQueryable<Product> DomainGetIQueryableDefaults(IQueryable<Product> query)
        {
            return query
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category);
        }
    }
}