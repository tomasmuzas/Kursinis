using System;
using EntityFrameworkMigrator.Entities;

namespace EntityFrameworkMigrator.Relationships.ReflectionRelationships
{
    public class ReflectedForeignKeyInfo
    {
        public Type FromEntity { get; set; }

        public TableInfo FromTable { get; set; }

        public string FromColumn { get; set; }

        public Type ToEntity { get; set; }

        public TableInfo ToTable { get; set; }

        public string ToColumn { get; set; }

    }
}
