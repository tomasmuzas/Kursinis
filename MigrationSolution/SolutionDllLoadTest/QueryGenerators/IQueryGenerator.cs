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
    }
}