using System;
using System.Reflection;
using EntityFrameworkMigrator.Entities;
using EntityFrameworkMigrator.Mappers;
using EntityFrameworkMigrator.QueryGenerators;
using EntityFrameworkMigrator.SqlHelpers;

namespace EntityFrameworkMigrator
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("No assembly supplied.");
                return 1; 
            }

            Console.WriteLine("Loading Assembly.");
            var assembly = Assembly.LoadFrom(args[0]);
            Console.WriteLine("Assembly loaded successfully.");

            var entitiesBuilder = new EntityInformationBuilder(new SqlServerHelper(), new RelationshipMapper());

            Console.WriteLine("Fetching entity information and relationships.");
            var entityMap = entitiesBuilder.BuildEntityDatabaseMapFromAssembly(assembly);
            Console.WriteLine("Entity information and relationships fetched successfully");

            var migrationScriptGenerator = new MigrationScriptGenerator(new SqlServerQueryGenerator());
            Console.Write(migrationScriptGenerator.GenerateMigrationScript(entityMap));

            return 0;
        }
    }
}
