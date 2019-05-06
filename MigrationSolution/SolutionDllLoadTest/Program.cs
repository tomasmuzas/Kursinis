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

            Console.WriteLine("Loading Assembly.");
            var assembly = Assembly.LoadFrom(args[0]);
            Console.WriteLine("Assembly loaded successfully.");

            IQueryGenerator generator = new SqlServerQueryGenerator();

            var entitiesBuilder = new EntityInformationBuilder(new SqlServerHelper(), new RelationshipMapper());

            Console.WriteLine("Fetching entity information and relationships.");
            var entitiesInfo = entitiesBuilder.BuildEntityInformationFromAssembly(assembly);
            Console.WriteLine("Entity information and relationships fetched successfully");


            foreach (var entityInformation in entitiesInfo)
            {
                var tableInfo = entityInformation.TableInformation;

                Console.WriteLine("Entity:");
                Console.WriteLine(entityInformation.ClrType.Name);
                Console.WriteLine("Table name:");
                Console.WriteLine(entityInformation.TableInformation.Name);
                Console.WriteLine("Primary Keys:");

                foreach (var primaryKey in entityInformation.PrimaryKeys)
                {
                    Console.WriteLine(generator.GeneratePrimaryKeyRenameQuery(primaryKey.Name, tableInfo.Schema, tableInfo.Name));
                }

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

                Console.WriteLine("Indices:");

                foreach (var index in entityInformation.Indices)
                {
                    Console.WriteLine(generator.GenerateIndexRenameQuery(
                        index.Name, 
                        index.Table.Schema, 
                        index.Table.Name, 
                        index.ColumnName));
                }
                Console.WriteLine();
            }

            return 0;
        }
    }
}
