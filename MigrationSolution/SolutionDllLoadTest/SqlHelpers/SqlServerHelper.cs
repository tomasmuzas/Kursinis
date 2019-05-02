using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SolutionDllLoadTest.Relationships.DatabaseRelationships;

namespace SolutionDllLoadTest.SqlHelpers
{
    public class SqlServerHelper : ISqlHelper
    {
        public IEnumerable<DatabaseForeignKeyInfo> GetForeignKeyInformation(
            DbContext databaseContext,
            string schema,
            string fromTableName)
        {
            var query = 
                $@"SELECT  obj.name AS [Name],
                    sch.name AS [Schema],
                    tab1.name AS [FromTable],
                    col1.name AS [FromColumn],
                    tab2.name AS [ToTable],
                    col2.name AS [ToColumn]
                FROM sys.foreign_key_columns fkc
                INNER JOIN sys.objects obj
                    ON obj.object_id = fkc.constraint_object_id
                INNER JOIN sys.tables tab1
                    ON tab1.object_id = fkc.parent_object_id
                INNER JOIN sys.schemas sch
                    ON tab1.schema_id = sch.schema_id
                INNER JOIN sys.columns col1
                    ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id
                INNER JOIN sys.tables tab2
                    ON tab2.object_id = fkc.referenced_object_id
                INNER JOIN sys.columns col2
                    ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id
                WHERE sch.name = '{schema}' 
                    AND tab1.name = '{fromTableName}'";

            return databaseContext.Database
                .SqlQuery<DatabaseForeignKeyInfo>(query)
                .ToList();
        }

        public IEnumerable<DatabasePrimaryKeyInfo> GetPrimaryKeyInformation(
            DbContext databaseContext,
            string tableName)
        {
            var query = $@"SELECT column_name as ""Column"", TC.CONSTRAINT_NAME as Name
                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                            INNER JOIN
                                INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
                                      ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
                                         TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME AND 
                                         KU.table_name='{tableName}'
                            ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;";
            return databaseContext.Database
                .SqlQuery<DatabasePrimaryKeyInfo>(query)
                .ToList();
        }
    }
}
