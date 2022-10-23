using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ServiceBrick;
using ServiceBrick.Storage.EntityFrameworkCore;

namespace Example.EntityFrameworkCore
{
    // dotnet ef migrations add ExampleV1 --context ExampleContext --startup-project ../WebApp

    public partial class ExampleContext : DbContext, IDesignTimeDbContextFactory<ExampleContext>
    {
        private DbContextOptions<ExampleContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExampleContext()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<ExampleContext>();
            string connectionString = configuration.GetEntityFrameworkCoreConnectionString(
                ExampleEntityFrameworkCoreConstants.APPSETTING_DATABASE_CONNECTION);
            builder.UseSqlServer(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(ExampleContext).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            _options = builder.Options;
        }

        public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
        {
            _options = options;
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set default schema
            builder.HasDefaultSchema(ExampleEntityFrameworkCoreConstants.DATABASE_SCHEMA_NAME);

            builder.Entity<Product>(b =>
            {
                b.HasKey(x => x.ID);

                b.HasMany(e => e.ProductCategories)
                    .WithOne(e => e.Product)
                    .HasForeignKey(fk => fk.ProductID);
            });

            builder.Entity<Category>(b =>
            {
                b.HasKey(x => x.ID);

                b.HasMany(e => e.ProductCategories)
                    .WithOne(e => e.Category)
                    .HasForeignKey(fk => fk.CategoryID);
            });

            builder.Entity<ProductCategory>().HasKey(key => new { key.ProductID, key.CategoryID });
        }

        public virtual ExampleContext CreateDbContext(string[] args)
        {
            return new ExampleContext(_options);
        }
    }
}