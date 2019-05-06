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

namespace EntityFrameworkMigrator
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("ERROR: No assembly supplied.");
                return 1; 
            }

            Console.WriteLine("Loading Assembly.");
            var assembly = Assembly.LoadFrom(args[0]);
            Console.WriteLine("Assembly loaded successfully.");

            var dbContextType = assembly.DefinedTypes
                .Single(t => t.IsSubclassOf(typeof(DbContext)));

            var dbContextInstance = (DbContext)Activator.CreateInstance(dbContextType);
            DbProviderFactory factory = dbContextInstance.GetDatabaseProviderFactory();

            ISqlHelper helper;
            try
            {
                helper = SqlQueryProviderLocator.ResolveSqlHelper(factory);
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("ERROR: Cannot get entity information. Unsupported DBMS type");
                return 1;
            }

            var entitiesBuilder = new EntityInformationBuilder(helper, new RelationshipMapper());

            Console.WriteLine("Fetching entity information and relationships.");
            var entityMap = entitiesBuilder.BuildEntityDatabaseMap(dbContextInstance);
            Console.WriteLine("Entity information and relationships fetched successfully");

            IQueryGenerator generator;
            try
            {
                generator = SqlQueryProviderLocator.ResolveQueryProvider(factory);
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("ERROR: Cannot generate database migration script. Unsupported DBMS type");
                return 1;
            }

            var migrationScriptGenerator = new MigrationScriptGenerator(generator);
            Console.Write(migrationScriptGenerator.GenerateMigrationScript(entityMap));

            return 0;
        }
    }
}
