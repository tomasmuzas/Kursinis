using System;
using System.IO;
using EntityFrameworkMigrator.MigrationSteps;

namespace EntityFrameworkMigrator
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("ERROR: No project supplied.");
                Console.ResetColor();
                return 1; 
            }

            var projectName = PrepareProjectFolderStep.GetProjectName(args[0]).Replace(".csproj", string.Empty);

            var createDatabaseMigrationStep = new CreateDatabaseMigrationStep();

            Console.WriteLine("Loading Assembly.");
            createDatabaseMigrationStep.LoadAssembly(Path.Combine(args[0], "bin", "Debug", $"{projectName}.dll"));
            PrintSuccessMessage("Assembly loaded successfully.");

            Console.WriteLine("Fetching entity information and relationships.");
            var map = createDatabaseMigrationStep.BuildEntityMap();
            PrintSuccessMessage("Entity information and relationships fetched successfully.");

            Console.WriteLine("Generating migration SQL.");
            var migrationScript = createDatabaseMigrationStep.CreateMigrationSql(map);
            PrintSuccessMessage("Migration SQL generated successfully.");

            Console.WriteLine("Testing generated script on database. Transaction will be rolled back.");
            createDatabaseMigrationStep.TestSql(migrationScript);
            PrintSuccessMessage("Generated script was successfully tested against the database. Nothing was changed, transaction rolled back successfully.");


            var dbContextName = createDatabaseMigrationStep.DbContextType.Name;

            var prepareProjectStep = new PrepareProjectFolderStep(args[0]);

            Console.WriteLine("Creating Project Copy.");
            prepareProjectStep.CreateProjectCopy();
            PrintSuccessMessage("Successfully created project copy.");

            Console.WriteLine("Building new .NET Core project file.");
            prepareProjectStep.CreateNewProjectFile();
            PrintSuccessMessage("Successfully built new .NET Core project file.");

            Console.WriteLine("Replacing Entity Framework namespaces to Entity Framework core.");
            prepareProjectStep.AdjustEntityFrameworkNamespaces();
            PrintSuccessMessage("Namespaces successfully replaced");

            Console.WriteLine("Adapting Entity Framework DbContext to Core");
            prepareProjectStep.SetupEntityFrameworkCoreDbContext(dbContextName);
            PrintSuccessMessage("DbContext adapted.");

            PrintSuccessMessage("Migration was successful! Press any key to quit...");
            Console.Read();
            return 0;
        }

        public static void PrintSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
