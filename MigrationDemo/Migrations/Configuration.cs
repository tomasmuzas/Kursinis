using MigrationDemo.Entities;

namespace MigrationDemo.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        // TODO: Will change in EF Core
        protected override void Seed(DatabaseContext context)
        {
            context.Products.Add(new DbProduct
            {
                Id = 1,
                Name = "Product1",
                Isin = "US5949181045"
            });
        }
    }
}
