using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;

namespace EntityFrameworkMigrator.Extensions
{
    public static class DbContextExtensions
    {
        public static MetadataWorkspace GetMetadata(this DbContext dbContext)
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            var metadata = objectContext.MetadataWorkspace;

            return metadata;
        }

        public static string GetDatabaseName(this DbContext dbContext)
        {
            dbContext.Database.Connection.Open();
            var name = dbContext.Database.Connection.Database;
            dbContext.Database.Connection.Close();
            return name;
        }
    }
}
