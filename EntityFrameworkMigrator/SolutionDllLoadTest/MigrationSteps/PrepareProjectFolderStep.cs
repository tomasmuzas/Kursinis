using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using EntityFrameworkMigrator.IO;

namespace EntityFrameworkMigrator.MigrationSteps
{
    public class PrepareProjectFolderStep
    {
        private string ProjectPath { get; }

        private string NewProjectPath => ProjectPath + "_Dotnet_Core";

        public PrepareProjectFolderStep(string projectPath)
        {
            ProjectPath = projectPath;
        }

        public void CreateProjectCopy()
        {
            var projectName = GetProjectName(ProjectPath);
            FileHelper.CopyDirectory(
                ProjectPath, 
                NewProjectPath, 
                new []{"packages.config", $"{projectName}.csproj"}, 
                new [] {"bin", "obj", "packages", "Properties", "Migrations"});
        }

        public void CreateNewProjectFile()
        {
            var projectName = GetProjectName(ProjectPath);
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

            var packages = GetProjectPackages();
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

            xml.Save(Path.Combine(NewProjectPath, projectName));
        }

        public void SetupEntityFrameworkCoreDbContext(string dbContextName)
        {
            var directory = FileHelper.GetDirectory(ProjectPath);
            var files = FileHelper.GetFiles(directory);

            var dbContextFile = files.Single(f => f.Name == dbContextName + ".cs");
            var dbContextText = File.ReadAllText(Path.Combine(NewProjectPath, dbContextFile.Name));

            var replaced = Regex.Replace(dbContextText,
                @":(\s|\r|\n)*base\s*\(\""([\w=\s\\;\(\)]+)\""\)",
                @": base(
new DbContextOptionsBuilder().UseSqlServer(""$2"").Options)");

            File.WriteAllText(Path.Combine(NewProjectPath, dbContextFile.Name), replaced);
        }

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

        public void AdjustEntityFrameworkNamespaces()
        {
            FileHelper.ReplaceInFolder(
                NewProjectPath, 
                "System.Data.Entity", 
                "Microsoft.EntityFrameworkCore", 
                ignoreFolders: new []{"Migrations"});
        }

        private IEnumerable<(string, string)> GetProjectPackages()
        {
            var doc = new XmlDocument();
            doc.Load(ProjectPath + "\\packages.config");
            var nodes = doc.DocumentElement.SelectNodes("/packages/package");
            return nodes.Cast<XmlNode>()
                .Select(n => (n.Attributes["id"].Value, n.Attributes["version"].Value));
        }
    }
}
