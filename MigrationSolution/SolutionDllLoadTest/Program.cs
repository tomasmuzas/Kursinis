using System;
using EntityFrameworkMigrator.MigrationSteps;

namespace EntityFrameworkMigrator
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("ERROR: No assembly supplied.");
                return 1; 
            }

            CreateDatabaseMigrationStep.CreateMigrationSql(args[0]);

            PrepareProjectFolderStep.CreateProjectCopy(args[1]);
            
            return 0;
        }
    }
}
