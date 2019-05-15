using System;

namespace EntityFrameworkMigrator.Entities
{
    public class Index
    {
        public string Name { get; set; }

        public TableInfo Table { get; set; }

        public string ColumnName { get; set; }

        public string EfCoreColumnName
        {
            get
            {
                if (ColumnName.EndsWith("_id", StringComparison.OrdinalIgnoreCase))
                {
                    return ColumnName.Replace("_", "");
                }

                return ColumnName;
            }
        }
    }
}
