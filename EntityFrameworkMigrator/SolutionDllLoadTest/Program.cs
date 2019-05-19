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

            PrepareProjectFolderStep.CreateProjectCopy(args[0]);
            PrepareProjectFolderStep.CreateNewProjectFile(args[0]);
            PrepareProjectFolderStep.AdjustEntityFrameworkNamespaces(args[0]);

            Console.Read();
            return 0;
        }
    }
}
