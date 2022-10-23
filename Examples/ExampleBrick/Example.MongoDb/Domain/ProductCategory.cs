using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBrick.Storage.MongoDb;
using System.Linq.Expressions;

namespace Example.MongoDb
{
    public partial class ProductCategory : MongoDbDomainObject<ProductCategory>
    {
        public ProductCategory()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        public virtual string CategoryId { get; set; }
        public virtual string ProductId { get; set; }

        public override Expression<Func<ProductCategory, bool>> DomainGetItemFilter(ProductCategory obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}