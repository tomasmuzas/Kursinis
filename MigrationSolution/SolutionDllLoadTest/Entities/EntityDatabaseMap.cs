using System.Collections.Generic;

namespace SolutionDllLoadTest.Entities
{
    public class EntityDatabaseMap
    {
        public string DatabaseName { get; set; }

        public IEnumerable<EntityInformation> EntityInformation { get; set; }
    }
}
