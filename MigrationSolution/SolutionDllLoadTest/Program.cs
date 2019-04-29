using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using SolutionDllLoadTest.Extensions;
using SolutionDllLoadTest.QueryGenerators;

namespace SolutionDllLoadTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.LoadFrom("C:\\Users\\BD1892\\source\\repos\\Kursinis\\MigrationDemo\\bin\\MigrationDemo.dll");
            var dbContextType = assembly.DefinedTypes
                .Single(t => t.IsSubclassOf(typeof(DbContext)));

            var dbContextInstance = (DbContext)Activator.CreateInstance(dbContextType);

            var metadata = dbContextInstance.GetMetadata();

            var entityTypes = metadata.GetEntityTypes();

            IQueryGenerator generator = new SqlServerQueryGenerator();

            foreach (var entityType in entityTypes)
            {
                Console.WriteLine("Entity:");
                Console.WriteLine(entityType.ToString());
                Console.WriteLine("Table name:");
                Console.WriteLine(metadata.GetTableInfo(entityType).Name);
                Console.WriteLine("Keys:");
                Console.WriteLine(string.Join(",", metadata.GetKeyNames(entityType)));
                Console.WriteLine("Foreign Keys:");
                var foreignKeys = metadata.GetForeignKeys(entityType);

                foreach (var foreignKey in foreignKeys)
                {
                    var fromTableInfo = metadata.GetTableInfo(foreignKey.FromEntity);
                    var toTableInfo = metadata.GetTableInfo(foreignKey.ToEntity);
                    var fkQuery = generator.GenerateGetAllForeignKeysForTableQuery(fromTableInfo.Schema, fromTableInfo.Name, foreignKey.FromColumn, toTableInfo.Name, foreignKey.ToColumn);
                    var fk = dbContextInstance.Database.SqlQuery<string>(fkQuery).SingleOrDefault();
                    if (fk != null)
                    {
                        Console.WriteLine(generator.GenerateForeignKeyRenameQuery(fk, fromTableInfo.Name, toTableInfo.Name, foreignKey.FromColumn));
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
