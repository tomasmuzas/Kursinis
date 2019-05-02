namespace SolutionDllLoadTest.QueryGenerators
{
    public class SqlServerQueryGenerator : IQueryGenerator
    {
        public string GenerateForeignKeyRenameQuery(
            string schema,
            string originalForeignKeyName, 
            string fromTableName, 
            string toTableName, 
            string fromColumn)
        {
            return $"EXEC sp_rename N'[{schema}].[{originalForeignKeyName}]', N'FK_{fromTableName}_{toTableName}_{fromColumn}'";
        }

        public string GeneratePrimaryKeyRenameQuery(string originalName, string schema, string tableName)
        {
            return $"EXEC sp_rename N'[{schema}].[{tableName}].[{originalName}]',N'PK_{tableName}'";
        }
    }
}
