namespace SolutionDllLoadTest.QueryGenerators
{
    public class SqlServerQueryGenerator : IQueryGenerator
    {
        public string GenerateGetAllForeignKeysForTableQuery(
            string schema, 
            string fromTableName, 
            string fromColumn, 
            string toTableName, 
            string toColumn)
        {
            return $@"SELECT  obj.name AS FK_NAME
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
                AND tab1.name = '{fromTableName}' AND col1.name = '{fromColumn}'
                AND tab2.name = '{toTableName}' AND col2.name = '{toColumn}'";
        }

        public string GenerateForeignKeyRenameQuery(
            string originalForeignKeyName, 
            string fromTableName, 
            string toTableName, 
            string fromColumn)
        {
            return $"EXEC sp_rename N'{originalForeignKeyName}', N'FK_{fromTableName}_{toTableName}_{fromColumn}'";
        }
    }
}
