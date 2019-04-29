using System;
using System.Collections.Generic;
using SolutionDllLoadTest.Relationships.ReflectionRelationships;

namespace SolutionDllLoadTest.Entities
{
    public class EntityInformation
    {
        public Type ClrType { get; set; }

        public TableInfo TableInformation { get; set; }

        public IEnumerable<ReflectedPrimaryKey> PrimaryKeys { get; set; }

        public IEnumerable<ForeignKey> ForeignKeys { get; set; }
    }
}
