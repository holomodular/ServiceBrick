using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBrick;
using ServiceBrick.Storage.MongoDb;
using System.Linq.Expressions;

namespace Example.MongoDb
{
    public partial class Product : MongoDbDomainObject<Product>, IDpCreateDate, IDpUpdateDate
    {
        public Product()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }
        public virtual DateTimeOffset CreateDate { get; set; }
        public virtual DateTimeOffset UpdateDate { get; set; }
        public virtual DateTimeOffset? ExpirationDate { get; set; }
        public bool IsExpired { get; set; }

        public override Expression<Func<Product, bool>> DomainGetItemFilter(Product obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}