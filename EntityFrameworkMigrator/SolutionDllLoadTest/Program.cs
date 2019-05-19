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
                Console.WriteLine("ERROR: No project supplied.");
                return 1; 
            }

            var projectName = PrepareProjectFolderStep.GetProjectName(args[0]).Replace(".csproj", string.Empty);

            CreateDatabaseMigrationStep.CreateMigrationSql(Path.Combine(args[0], "bin", "Debug", $"{projectName}.dll"));

            var prepareProjectStep = new PrepareProjectFolderStep(args[0]);

            Console.WriteLine("Creating Project Copy.");
            prepareProjectStep.CreateProjectCopy();
            Console.WriteLine("Successfully created project copy.");

            Console.WriteLine("Building new .NET Core project file.");
            prepareProjectStep.CreateNewProjectFile();
            Console.WriteLine("Successfully built new .NET Core project file.");

            Console.WriteLine("Replacing Entity Framework namespaces to Entity Framework core.");
            prepareProjectStep.AdjustEntityFrameworkNamespaces();
            Console.WriteLine("Namespaces successfully replaced");

            Console.Read();
            return 0;
        }
    }
}
