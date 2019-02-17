using System.ComponentModel.DataAnnotations;

namespace DotNetCoreWebApi.Entities
{
    public class DbInvestmentAgreement
    {
        [Key]
        public int Id { get; set; }

        public DbProduct Product { get; set; }
    }
}