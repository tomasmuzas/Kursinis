namespace SolutionDllLoadTest.QueryGenerators
{
    public interface IQueryGenerator
    {
        string GenerateForeignKeyRenameQuery(
            string schema,
            string originalForeignKeyName, 
            string fromTableName, 
            string toTableName, 
            string fromColumn);

        string GeneratePrimaryKeyRenameQuery(string originalName, string schema, string tableName);

        string GenerateIndexRenameQuery(string originalName, string schema, string tableName, string columnName);
    }
}