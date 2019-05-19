using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntityFrameworkMigrator.IO
{
    public class FileHelper
    {
        public static void ReplaceInFolder(
            string path, 
            string oldText, 
            string newText, 
            IEnumerable<string> ignoreFiles = null, 
            IEnumerable<string> ignoreFolders = null)
        {
            DoActionInFolder(path, file =>
            {
                var text = File.ReadAllText(file.FullName);
                if (!text.Contains(oldText))
                {
                    return;
                }

                var replacedText = text.Replace(oldText, newText);
                File.WriteAllText(file.FullName, replacedText);
            }, ignoreFiles, ignoreFolders);
        }


        public static void CopyDirectory(string source, string destination, IEnumerable<string> ignoredFiles = null, IEnumerable<string> ignoredFolders = null)
        {
            var currentDirectory = GetDirectory(source);

            var directoriesInFolder = currentDirectory.GetDirectories().AsEnumerable();
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            var files = GetFiles(currentDirectory);

            if (ignoredFiles != null)
            {
                files = files.Where(f => !ignoredFiles.Contains(f.Name));
            }

            foreach (var file in files)
            {
                var temppath = Path.Combine(destination, file.Name);
                file.CopyTo(temppath, false);
            }

            if (ignoredFolders != null)
            {
                directoriesInFolder = directoriesInFolder.Where(f => !ignoredFolders.Contains(f.Name));
            }
            foreach (DirectoryInfo subdir in directoriesInFolder)
            {
                var temppath = Path.Combine(destination, subdir.Name);
                CopyDirectory(subdir.FullName, temppath);
            }
        }

        private static void DoActionInFolder(
            string path,
            Action<FileInfo> actionWithFile,
            IEnumerable<string> ignoredFiles = null,
            IEnumerable<string> ignoredFolders = null)
        {
            var directory = GetDirectory(path);
            var files = GetFiles(directory)
                .Where(f => !ignoredFiles?.Contains(f.Name) ?? true);

            foreach (var file in files)
            {
                actionWithFile?.Invoke(file);
            }

            var innerDirectories = directory.GetDirectories()
                .Where(d => !ignoredFolders?.Contains(d.Name) ?? true);
            foreach (var innerDirectory in innerDirectories)
            {
                DoActionInFolder(innerDirectory.FullName, actionWithFile, ignoredFiles, ignoredFolders);
            }
        }

        public static DirectoryInfo GetDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Project folder does not exist or could not be found: "
                    + path);
            }

            return directory;
        }

        public static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Project folder does not exist or could not be found: "
                    + directory.Name);
            }

            return directory.GetFiles().AsEnumerable();
        }
    }
}
