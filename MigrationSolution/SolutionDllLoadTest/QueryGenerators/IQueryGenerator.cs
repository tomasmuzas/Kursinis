namespace SolutionDllLoadTest.QueryGenerators
{
    public interface IQueryGenerator
    {
        string GenerateGetAllForeignKeysForTableQuery(
            string schema, 
            string fromTableName, 
            string fromColumn, 
            string toTableName, 
            string toColumn);

        string GenerateForeignKeyRenameQuery(
            string originalForeignKeyName, 
            string fromTableName, 
            string toTableName, 
            string fromColumn);
    }
}