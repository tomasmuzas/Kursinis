using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using EntityFrameworkMigrator.Entities;
using EntityFrameworkMigrator.Extensions;
using EntityFrameworkMigrator.Mappers;
using EntityFrameworkMigrator.QueryGenerators;
using EntityFrameworkMigrator.Services;
using EntityFrameworkMigrator.SqlHelpers;

namespace EntityFrameworkMigrator.MigrationSteps
{
    public class CreateDatabaseMigrationStep
    {
        public static void CreateMigrationSql(string assemblyPath)
        {
            Console.WriteLine("Loading Assembly.");
            var assembly = Assembly.LoadFrom(assemblyPath);
            Console.WriteLine("Assembly loaded successfully.");

            var dbContextType = assembly.DefinedTypes
                .Single(t => t.IsSubclassOf(typeof(DbContext)));

            var dbContextInstance = (DbContext)Activator.CreateInstance(dbContextType);
            DbProviderFactory factory = dbContextInstance.GetDatabaseProviderFactory();
            try
            {
                SqlQueryProviderLocator.AssertSupported(factory);
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("ERROR: Unsupported DBMS type");
                throw new Exception();
            }

            ISqlHelper helper = SqlQueryProviderLocator.ResolveSqlHelper(factory);

            var entitiesBuilder = new EntityInformationBuilder(helper, new RelationshipMapper());

            Console.WriteLine("Fetching entity information and relationships.");
            var entityMap = entitiesBuilder.BuildEntityDatabaseMap(dbContextInstance);
            Console.WriteLine("Entity information and relationships fetched successfully");

            IQueryGenerator generator = SqlQueryProviderLocator.ResolveQueryProvider(factory);

            var migrationScriptGenerator = new MigrationScriptGenerator(generator);
            var migrationScript = migrationScriptGenerator.GenerateMigrationScript(entityMap);
            Console.Write(migrationScript);

            using (var transaction = dbContextInstance.Database.BeginTransaction())
            {
                try
                {
                    dbContextInstance.Database.ExecuteSqlCommand(migrationScript);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: Generated script was not successfully tested against the database.");
                    throw new Exception();
                }
                finally
                {
                    transaction.Rollback();
                }
                Console.WriteLine("Generated script was successfully tested against the database. Nothing was changed, transaction rolled back successfully.");
            }
        }
    }
}
