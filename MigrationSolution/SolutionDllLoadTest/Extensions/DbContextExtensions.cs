using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;

namespace SolutionDllLoadTest.Extensions
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
            return dbContext.Database.Connection.Database;
        }
    }
}
