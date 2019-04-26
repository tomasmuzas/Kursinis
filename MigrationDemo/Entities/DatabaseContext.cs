using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MigrationDemo.Entities
{
    using System.Data.Entity;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("data source=(LocalDb)\\MSSQLLocalDB;integrated security=True;")
        {
        }

        public virtual DbSet<DbInvestmentAgreement> InvestmentAgreements { get; set; }

        public virtual DbSet<DbProfile> Profiles { get; set; }

        public virtual DbSet<DbProduct> Products { get; set; }
    }
}