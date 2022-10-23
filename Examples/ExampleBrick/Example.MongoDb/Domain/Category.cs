using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceBrick;
using ServiceBrick.Storage.MongoDb;
using System.Linq.Expressions;

namespace Example.MongoDb
{
    public partial class Category : MongoDbDomainObject<Category>, IDpCreateDate, IDpUpdateDate
    {
        public Category()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }
        public virtual DateTimeOffset CreateDate { get; set; }
        public virtual DateTimeOffset UpdateDate { get; set; }

        public override Expression<Func<Category, bool>> DomainGetItemFilter(Category obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}