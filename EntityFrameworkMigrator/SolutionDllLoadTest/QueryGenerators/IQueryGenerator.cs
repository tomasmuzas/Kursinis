namespace EntityFrameworkMigrator.QueryGenerators
{
    public interface IQueryGenerator
    {
        string GenerateTableRenameQuery(string schema, string oldName, string newName);

        string GenerateForeignKeyRenameQuery(
            string schema,
            string originalForeignKeyName, 
            string fromTableName, 
            string toTableName, 
            string fromColumn);

        string GenerateColumnRenameQuery(string originalName, string newName, string schema, string tableName);

        string GeneratePrimaryKeyRenameQuery(string originalName, string schema, string tableName);

        string GenerateIndexRenameQuery(string originalName, string schema, string tableName, string columnName);

        string GenerateDatabaseSelectionQuery(string databaseName);

        string BeginTransaction { get; }

        string CommitTransaction { get; }

        string RollbackTransaction { get; }

        string Comment { get; }
    }
}