using System.Collections.Generic;
using System.Linq;
using SolutionDllLoadTest.Entities;
using SolutionDllLoadTest.Relationships.DatabaseRelationships;
using SolutionDllLoadTest.Relationships.ReflectionRelationships;
using SolutionDllLoadTest.SqlHelpers;

namespace SolutionDllLoadTest.Mappers
{
    public class RelationshipMapper
    {
        public IEnumerable<ForeignKey> MapForeignKeys(
            IEnumerable<DatabaseForeignKeyInfo> databaseForeignKeys,
            IEnumerable<ReflectedForeignKeyInfo> reflectedForeignKeys)
        {
            var foreignKeys = new List<ForeignKey>();

            foreach (var reflectedForeignKey in reflectedForeignKeys)
            {
                var matchingDatabaseForeignKey = databaseForeignKeys
                    .SingleOrDefault(dbfk => dbfk.FromTable == reflectedForeignKey.FromTable.Name
                                    && dbfk.FromColumn == reflectedForeignKey.FromColumn
                                    && dbfk.ToTable == reflectedForeignKey.ToTable.Name
                                    && dbfk.ToColumn == reflectedForeignKey.ToColumn);

                if (matchingDatabaseForeignKey != null)
                {
                    var foreignKey = new ForeignKey
                    {
                        Name = matchingDatabaseForeignKey.Name,
                        Schema = matchingDatabaseForeignKey.Schema,
                        FromTable = reflectedForeignKey.FromTable,
                        FromColumn = matchingDatabaseForeignKey.FromColumn,
                        ToTable = reflectedForeignKey.ToTable,
                        ToColumn = matchingDatabaseForeignKey.ToColumn
                    };

                    foreignKeys.Add(foreignKey);
                }
            }

            return foreignKeys;
        }

        public IEnumerable<PrimaryKey> MapPrimaryKeys(
            IEnumerable<DatabasePrimaryKeyInfo> databasePrimaryKeys,
            IEnumerable<string> reflectedPrimaryKeys)
        {
            var foreignKeys = new List<PrimaryKey>();

            foreach (var reflectedPrimaryKey in reflectedPrimaryKeys)
            {
                var matchingDatabasePrimaryKey = databasePrimaryKeys
                    .SingleOrDefault(dbpk => dbpk.Column == reflectedPrimaryKey);

                if (matchingDatabasePrimaryKey != null)
                {
                    var primaryKey = new PrimaryKey
                    {
                        Name = matchingDatabasePrimaryKey.Name,
                        ColumnName = matchingDatabasePrimaryKey.Column
                    };

                    foreignKeys.Add(primaryKey);
                }
            }

            return foreignKeys;
        }
    }
}
