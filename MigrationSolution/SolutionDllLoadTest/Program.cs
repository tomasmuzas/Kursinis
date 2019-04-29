using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using SolutionDllLoadTest.Extensions;
using SolutionDllLoadTest.QueryGenerators;
using SolutionDllLoadTest.SqlHelpers;

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

            var entitiesInfo = metadata.GetEntitiesInformation();

            IQueryGenerator generator = new SqlServerQueryGenerator();
            var helper = new SqlServerHelper();

            foreach (var entityInformation in entitiesInfo)
            {
                Console.WriteLine("Entity:");
                Console.WriteLine(entityInformation.ClrType.Name);
                Console.WriteLine("Table name:");
                Console.WriteLine(entityInformation.TableInformation.Name);
                Console.WriteLine("Keys:");
                Console.WriteLine(string.Join(",", metadata.GetKeyNames(entityInformation.ClrType)));
                Console.WriteLine("Foreign Keys:");

                var infos = helper.GetForeignKeyInformation(
                    dbContextInstance, 
                    entityInformation.TableInformation.Schema, 
                    entityInformation.TableInformation.Name);

                foreach (var foreignKeyInfo in infos)
                {
                    Console.WriteLine(generator.GenerateForeignKeyRenameQuery(
                        foreignKeyInfo.Schema,
                        foreignKeyInfo.DatabaseName, 
                        foreignKeyInfo.FromTable, 
                        foreignKeyInfo.ToTable, 
                        foreignKeyInfo.FromColumn));
                }
                Console.WriteLine();
            }
        }
    }
}
