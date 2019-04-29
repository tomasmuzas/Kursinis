namespace SolutionDllLoadTest.QueryGenerators
{
    public interface IQueryGenerator
    {
        string GenerateForeignKeyRenameQuery(
            string originalForeignKeyName, 
            string fromTableName, 
            string toTableName, 
            string fromColumn);
    }
}