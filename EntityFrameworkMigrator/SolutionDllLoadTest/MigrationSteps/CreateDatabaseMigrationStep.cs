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
        public TypeInfo DbContextType { get; set; }

        private DbContext DbContextInstance { get; set; }

        public void LoadAssembly(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            DbContextType = assembly.DefinedTypes
                .Single(t => t.IsSubclassOf(typeof(DbContext)));
        }

        public EntityDatabaseMap BuildEntityMap()
        {
            DbContextInstance = (DbContext)Activator.CreateInstance(DbContextType);
            DbProviderFactory factory = DbContextInstance.GetDatabaseProviderFactory();
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

            
            var entityMap = entitiesBuilder.BuildEntityDatabaseMap(DbContextInstance);

            return entityMap;
        }

        public string CreateMigrationSql(EntityDatabaseMap entityMap)
        {
            DbProviderFactory factory = DbContextInstance.GetDatabaseProviderFactory();
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

            IQueryGenerator generator = SqlQueryProviderLocator.ResolveQueryProvider(factory);

            var migrationScriptGenerator = new MigrationScriptGenerator(generator);
            var migrationScript = migrationScriptGenerator.GenerateMigrationScript(entityMap);
            return migrationScript;

        }

        public void TestSql(string sql)
        {
            using (var transaction = DbContextInstance.Database.BeginTransaction())
            {
                try
                {
                    DbContextInstance.Database.ExecuteSqlCommand(sql);
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
            }
        }
    }
}
