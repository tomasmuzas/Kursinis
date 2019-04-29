using System;
using System.Collections.Generic;

namespace SolutionDllLoadTest.RelationshipDTOs
{
    public class EntityInformation
    {
        public Type ClrType { get; set; }

        public TableInfo TableInformation { get; set; }

        public IEnumerable<ForeignKey> ForeignKeys { get; set; }
    }
}
