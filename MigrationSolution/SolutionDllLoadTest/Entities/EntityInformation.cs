using System;
using System.Collections.Generic;
using SolutionDllLoadTest.RelationshipDTOs;
using SolutionDllLoadTest.Relationships;

namespace SolutionDllLoadTest.Entities
{
    public class EntityInformation
    {
        public Type ClrType { get; set; }

        public TableInfo TableInformation { get; set; }

        public IEnumerable<PrimaryKey> PrimaryKeys { get; set; }

        public IEnumerable<ForeignKey> ForeignKeys { get; set; }
    }
}
