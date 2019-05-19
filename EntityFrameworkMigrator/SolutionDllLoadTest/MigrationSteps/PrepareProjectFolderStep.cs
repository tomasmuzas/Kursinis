using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace EntityFrameworkMigrator.MigrationSteps
{
    public class PrepareProjectFolderStep
    {
        public static void CreateProjectCopy(string projectPath)
        {
            var projectName = GetProjectName(projectPath);
            CopyDirectory(
                projectPath, 
                GetNewProjectPath(projectPath), 
                new []{"packages.config", $"{projectName}.csproj"}, 
                new [] {"bin", "obj", "packages", "Properties"});
        }

        public static void CreateNewProjectFile(string projectPath)
        {
            var projectName = GetProjectName(projectPath);
            var xml = new XmlDocument();
            
            // Project
            var project = xml.CreateElement("Project");
            project.SetAttribute("Sdk", "Microsoft.NET.Sdk");

            // Framework
            var propertyGroup = xml.CreateElement("PropertyGroup");

            var targetFramework = xml.CreateElement("TargetFramework");
            targetFramework.InnerText = "netcoreapp2.1";

            var outputType = xml.CreateElement("OutputType");
            outputType.InnerText = "EXE";

            propertyGroup.AppendChild(targetFramework);
            propertyGroup.AppendChild(outputType);

            var packages = GetPackages(projectPath);
            var itemGroup = xml.CreateElement("ItemGroup");

            foreach (var package in packages)
            {
                // Don't add any Entity Framework nuget packages
                if (package.Item1 != "EntityFramework" || package.Item1.StartsWith("EntityFramework."))
                {
                    var packageElement = xml.CreateElement("PackageReference");
                    packageElement.SetAttribute("Include", package.Item1);
                    packageElement.SetAttribute("Version", package.Item2);
                    itemGroup.AppendChild(packageElement);
                }
            }

            // "Install" Entity Framework Core
            var EFCorePackage = xml.CreateElement("PackageReference");
            EFCorePackage.SetAttribute("Include", "Microsoft.EntityFrameworkCore.SqlServer");
            EFCorePackage.SetAttribute("Version", "2.1.8");
            itemGroup.AppendChild(EFCorePackage);

            project.AppendChild(propertyGroup);
            project.AppendChild(itemGroup);

            xml.AppendChild(project);

            xml.Save(Path.Combine(GetNewProjectPath(projectPath), projectName));
        }

        private static string GetNewProjectPath(string projectPath) => projectPath + "_dotnet_core";

        public static string GetProjectName(string projectPath)
        {
            var projectDirectory = new DirectoryInfo(projectPath);
            if (!projectDirectory.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Project folder does not exist or could not be found: "
                    + projectPath);
            }

            var files = projectDirectory.GetFiles().AsEnumerable();
            return files
                .Where(f => f.Name.EndsWith(".csproj"))
                .Select(f => f.Name)
                .Single();
        }

        public static void AdjustEntityFrameworkNamespaces(string projectPath)
        {
            var path = GetNewProjectPath(projectPath);
            var directory = GetDirectory(path);
            var files = GetFiles(directory);

            foreach (var file in files)
            {
                var text = File.ReadAllText(file.FullName);
                if (!Regex.IsMatch(text, @"System\.Data\.Entity"))
                {
                    continue;
                }

                var replacedText = text.Replace("System.Data.Entity", "Microsoft.EntityFrameworkCore");
                File.WriteAllText(file.FullName, replacedText);
            }
        }

        private static IEnumerable<(string, string)> GetPackages(string projectPath)
        {
            var doc = new XmlDocument();
            doc.Load(projectPath + "\\packages.config");
            var nodes = doc.DocumentElement.SelectNodes("/packages/package");
            return nodes.Cast<XmlNode>()
                .Select(n => (n.Attributes["id"].Value, n.Attributes["version"].Value));
        }

        private static void CopyDirectory(string source, string destination, IEnumerable<string> ignoredFiles = null, IEnumerable<string> ignoredFolders = null)
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

        private static DirectoryInfo GetDirectory(string path)
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

        private static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory)
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
