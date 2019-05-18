using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using MigrationDemo.Entities;

namespace EFConsole
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("data source=(LocalDb)\\MSSQLLocalDB;integrated security=True;Database=MigrationTest;")
        {
            
        }

        public virtual DbSet<DbProduct> Products { get; set; }

        public virtual DbSet<DbProfile> Profiles { get; set; }

        public virtual DbSet<DbInvestmentAgreement> Agreements { get; set; }
    }
}
