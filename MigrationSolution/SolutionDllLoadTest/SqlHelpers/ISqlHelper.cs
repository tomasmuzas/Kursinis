using System.Collections.Generic;
using System.Data.Entity;

namespace SolutionDllLoadTest.SqlHelpers
{
    public interface ISqlHelper
    {
        IEnumerable<DatabaseForeignKeyInfo> GetForeignKeyInformation(
            DbContext databaseContext,
            string schema,
            string fromTableName);
    }
}