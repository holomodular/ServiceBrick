using Microsoft.Extensions.Logging;
using ServiceBrick.Storage.EntityFrameworkCore;

namespace Example.EntityFrameworkCore
{
    public class ExampleStorageRepository<TDomain> : EntityFrameworkCoreStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        public ExampleStorageRepository(ILoggerFactory logFactory, ExampleContext context)
            : base(logFactory)
        {
            _context = context;
            _dbSet = context.Set<TDomain>();
        }
    }
}