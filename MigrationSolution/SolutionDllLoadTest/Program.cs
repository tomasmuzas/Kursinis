using System;
using System.Reflection;
using SolutionDllLoadTest.Entities;
using SolutionDllLoadTest.Mappers;
using SolutionDllLoadTest.QueryGenerators;
using SolutionDllLoadTest.SqlHelpers;

namespace SolutionDllLoadTest
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

            var assembly = Assembly.LoadFrom(args[0]);

            IQueryGenerator generator = new SqlServerQueryGenerator();

            var entitiesBuilder = new EntityInformationBuilder(new SqlServerHelper(), new RelationshipMapper());
            var entitiesInfo = entitiesBuilder.BuildEntityInformationFromAssembly(assembly);

            foreach (var entityInformation in entitiesInfo)
            {
                Console.WriteLine("Entity:");
                Console.WriteLine(entityInformation.ClrType.Name);
                Console.WriteLine("Table name:");
                Console.WriteLine(entityInformation.TableInformation.Name);
                //Console.WriteLine("Keys:");
                //Console.WriteLine(string.Join(",", metadata.GetKeyNames(entityInformation.ClrType)));
                Console.WriteLine("Foreign Keys:");

                foreach (var foreignKey in entityInformation.ForeignKeys)
                {
                    Console.WriteLine(generator.GenerateForeignKeyRenameQuery(
                        foreignKey.Schema,
                        foreignKey.Name,
                        foreignKey.FromTable.Name,
                        foreignKey.ToTable.Name,
                        foreignKey.FromColumn));
                }
                Console.WriteLine();
            }

            return 0;
        }
    }
}
