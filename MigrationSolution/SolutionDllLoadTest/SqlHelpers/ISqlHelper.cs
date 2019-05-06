using System.Collections.Generic;
using System.Data.Entity;
using EntityFrameworkMigrator.Relationships.DatabaseRelationships;

namespace EntityFrameworkMigrator.SqlHelpers
{
    public interface ISqlHelper
    {
        IEnumerable<DatabaseForeignKeyInfo> GetForeignKeyInformation(
            DbContext databaseContext,
            string schema,
            string fromTableName);

        IEnumerable<DatabasePrimaryKeyInfo> GetPrimaryKeyInformation(
            DbContext databaseContext,
            string tableName);

        IEnumerable<DatabaseIndexInfo> GetIndexInformation(
            DbContext databaseContext,
            string tableName);
    }
}