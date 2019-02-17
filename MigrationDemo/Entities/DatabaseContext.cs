namespace MigrationDemo.Entities
{
    using System.Data.Entity;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public virtual DbSet<DbInvestmentAgreement> InvestmentAgreements { get; set; }

        public virtual DbSet<DbProfile> Profiles { get; set; }
    }
}