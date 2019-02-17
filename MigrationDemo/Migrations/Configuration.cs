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
        }
    }
}
