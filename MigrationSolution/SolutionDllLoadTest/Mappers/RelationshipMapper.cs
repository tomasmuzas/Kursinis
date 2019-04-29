using System.Collections.Generic;
using System.Linq;
using SolutionDllLoadTest.Entities;
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
    }
}
