using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MigrationDemo.Entities
{
    public class DbProfile
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Surname { get; set; }

        public string Email { get; set; }

        public List<DbInvestmentAgreement> InvestmentAgreements { get; set; }
    }
}