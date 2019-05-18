using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntityFrameworkMigrator.MigrationSteps
{
    public class PrepareProjectFolderStep
    {
        public static void CreateDotnetCoreProjectCopy(string projectPath)
        {
            CopyDirectory(projectPath, projectPath + "_dotnet_core", new string[]{}, new string[] { });
        }

        private static void CopyDirectory(string source, string destination, IEnumerable<string> ignoredFiles = null, IEnumerable<string> ignoredFolders = null)
        {
            var currentDirectory = new DirectoryInfo(source);

            if (!currentDirectory.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Project folder does not exist or could not be found: "
                    + source);
            }

            var directoriesInFolder = currentDirectory.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            var files = currentDirectory.GetFiles().AsEnumerable();

            if (ignoredFiles != null)
            {
                files = files.Where(f => !ignoredFiles.Contains(f.Name));
            }

            foreach (var file in files)
            {
                var temppath = Path.Combine(destination, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (DirectoryInfo subdir in directoriesInFolder)
            {
                var temppath = Path.Combine(destination, subdir.Name);
                CopyDirectory(subdir.FullName, temppath);
            }
        }
    }
}
