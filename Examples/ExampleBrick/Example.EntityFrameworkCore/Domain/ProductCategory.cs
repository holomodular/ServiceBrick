using ServiceBrick.Storage.EntityFrameworkCore;

namespace Example.EntityFrameworkCore
{
    public partial class ProductCategory : EntityFrameworkCoreDomainObject<ProductCategory>
    {
        public ProductCategory()
        {
        }

        public int ProductID { get; set; }
        public int CategoryID { get; set; }

        public Product Product { get; set; }
        public Category Category { get; set; }

        public override IQueryable<ProductCategory> DomainGetItemQueryable(IQueryable<ProductCategory> query, ProductCategory obj)
        {
            return query.Where(x => x.ProductID == obj.ProductID && x.CategoryID == obj.CategoryID);
        }
    }
}