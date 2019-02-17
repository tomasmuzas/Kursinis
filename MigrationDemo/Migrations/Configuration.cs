namespace MigrationDemo.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MigrationDemo.Entities.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        // TODO: Will change in EF Core
        protected override void Seed(MigrationDemo.Entities.DatabaseContext context)
        {
        }
    }
}
