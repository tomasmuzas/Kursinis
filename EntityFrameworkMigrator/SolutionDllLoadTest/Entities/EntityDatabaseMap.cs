using System.Collections.Generic;

namespace EntityFrameworkMigrator.Entities
{
    public class EntityDatabaseMap
    {
        public string DatabaseName { get; set; }

        public IEnumerable<EntityInformation> EntityInformation { get; set; }
    }
}
