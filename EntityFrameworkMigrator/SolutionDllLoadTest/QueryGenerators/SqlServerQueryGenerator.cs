namespace EntityFrameworkMigrator.QueryGenerators
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
            return $"EXEC sp_rename N'[{schema}].[{originalForeignKeyName}]', N'FK_{fromTableName}_{toTableName}_{fromColumn}';";
        }

        public string GenerateColumnRenameQuery(string originalName, string newName, string schema, string tableName)
        {
            return $"EXEC sp_rename N'{schema}.{tableName}.{originalName}', N'{newName}', 'COLUMN';";
        }

        public string GeneratePrimaryKeyRenameQuery(string originalName, string schema, string tableName)
        {
            return $"EXEC sp_rename N'[{schema}].[{tableName}].[{originalName}]',N'PK_{tableName}';";
        }

        public string GenerateIndexRenameQuery(string originalName, string schema, string tableName, string columnName)
        {
            return $"EXEC sp_rename N'{schema}.{tableName}.{originalName}',N'IX_{tableName}_{columnName}', N'INDEX';";
        }

        public string GenerateDatabaseSelectionQuery(string databaseName)
        {
            return $"USE {databaseName};";
        }

        public string BeginTransaction => "BEGIN TRAN";

        public string CommitTransaction => "COMMIT TRAN";

        public string RollbackTransaction => "ROLLBACK TRAN";

        public string Comment => "--";
    }
}
