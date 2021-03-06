﻿using System;
using System.Collections.Generic;

namespace EntityFrameworkMigrator.Entities
{
    public class EntityInformation
    {
        public Type ClrType { get; set; }

        public TableInfo TableInformation { get; set; }

        public IEnumerable<PrimaryKey> PrimaryKeys { get; set; }

        public IEnumerable<ForeignKey> ForeignKeys { get; set; }

        public IEnumerable<Index> Indices{ get; set; }
    }
}
