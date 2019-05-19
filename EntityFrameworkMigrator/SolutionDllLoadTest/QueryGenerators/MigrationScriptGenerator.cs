using System;
using System.Linq;
using System.Text;
using EntityFrameworkMigrator.Entities;

namespace EntityFrameworkMigrator.QueryGenerators
{
    public static class CommentStrings
    {
        public static string DatabaseSelection = "Select database";

        public static string RenameTables = "Entity Framework core uses Entity Set name as table name rather than Entity Type name. Rename tables accordingly.";

        public static string RenameIdColumns = "Foreign key ID columns must be renamed according to EF Core standards.";

        public static string RenameIndices = "Rename indices";

        public static string RenamePrimaryKeys = "Rename primary keys";

        public static string RenameForeignKeys = "Rename foreign keys";
    }

    public class MigrationScriptGenerator
    {
        private readonly IQueryGenerator _queryGenerator;

        public MigrationScriptGenerator(IQueryGenerator queryGenerator)
        {
            _queryGenerator = queryGenerator;
        }

        public string GenerateMigrationScript(EntityDatabaseMap entityMap)
        {
            var stringBuilder = new StringBuilder();
            var newline = Environment.NewLine;
            var comment = _queryGenerator.Comment;

            var databaseSelection = _queryGenerator.GenerateDatabaseSelectionQuery(entityMap.DatabaseName);

            stringBuilder.Append(comment + " " + CommentStrings.DatabaseSelection);
            stringBuilder.Append(newline);
            stringBuilder.Append(databaseSelection);
            stringBuilder.Append(newline);
            stringBuilder.Append(newline);

            stringBuilder.Append(comment + " " + CommentStrings.RenameTables);
            stringBuilder.Append(newline);

            foreach (var table in entityMap.EntityInformation.Select(e => e.TableInformation))
            {
                var tableRenameQuery = _queryGenerator.GenerateTableRenameQuery(table.Schema, table.Name, table.EFCoreName);
                stringBuilder.Append(tableRenameQuery);
                stringBuilder.Append(newline);
            }

            stringBuilder.Append(newline);

            stringBuilder.Append(comment + " " + CommentStrings.RenameIdColumns);
            stringBuilder.Append(newline);
            foreach (var foreignKey in entityMap.EntityInformation.SelectMany(e => e.ForeignKeys))
            {
                if (foreignKey.FromColumn != foreignKey.EfCoreFromColumnName)
                {
                    var columnRenameQuery = _queryGenerator.GenerateColumnRenameQuery(
                        foreignKey.FromColumn,
                        foreignKey.EfCoreFromColumnName,
                        foreignKey.Schema,
                        foreignKey.FromTable.EFCoreName);

                    stringBuilder.Append(columnRenameQuery);
                    stringBuilder.Append(newline);
                }
            }

            stringBuilder.Append(newline);

            stringBuilder.Append(comment + " " + CommentStrings.RenameIndices);
            stringBuilder.Append(newline);
            foreach (var index in entityMap.EntityInformation.SelectMany(e => e.Indices))
            {
                var indexRenameQuery = _queryGenerator.GenerateIndexRenameQuery(
                    index.Name,
                    index.Table.Schema,
                    index.Table.EFCoreName,
                    index.EfCoreColumnName);

                stringBuilder.Append(indexRenameQuery);
                stringBuilder.Append(newline);
            }
            stringBuilder.Append(newline);

            stringBuilder.Append(comment + " " + CommentStrings.RenamePrimaryKeys);
            stringBuilder.Append(newline);
            foreach (var primaryKey in entityMap.EntityInformation.SelectMany(e => e.PrimaryKeys))
            {
                var primaryKeyRenameQuery =
                    _queryGenerator.GeneratePrimaryKeyRenameQuery(
                        primaryKey.Name, 
                        primaryKey.Table.Schema, 
                        primaryKey.Table.EFCoreName);

                stringBuilder.Append(primaryKeyRenameQuery);
                stringBuilder.Append(newline);
            }
            stringBuilder.Append(newline);

            stringBuilder.Append(comment + " " + CommentStrings.RenameForeignKeys);
            stringBuilder.Append(newline);
            foreach (var foreignKey in entityMap.EntityInformation.SelectMany(e => e.ForeignKeys))
            {
                var indexRenameQuery = _queryGenerator.GenerateForeignKeyRenameQuery(
                    foreignKey.Schema,
                    foreignKey.Name,
                    foreignKey.FromTable.EFCoreName,
                    foreignKey.ToTable.EFCoreName,
                    foreignKey.EfCoreFromColumnName);

                stringBuilder.Append(indexRenameQuery);
                stringBuilder.Append(newline);
            }
            stringBuilder.Append(newline);

            return stringBuilder.ToString();
        }
    }
}
