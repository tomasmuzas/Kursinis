using System.Collections.Generic;
using System.Reflection;

namespace EntityFrameworkMigrator.Entities
{
    public class TableInfo
    {
        public string Schema { get; set; }

        public string Name { get; set; }

        public IDictionary<string, PropertyInfo> ColumnPropertyMap { get; set; }

        public string EFCoreName { get; set; }
    }
}
