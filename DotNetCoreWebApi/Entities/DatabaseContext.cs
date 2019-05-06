using Microsoft.EntityFrameworkCore;

namespace DotNetCoreWebApi.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbInvestmentAgreement> DbInvestmentAgreements { get; set; }

        public virtual DbSet<DbProfile> DbProfiles { get; set; }

        public virtual DbSet<DbProduct> DbProducts { get; set; }
    }
}