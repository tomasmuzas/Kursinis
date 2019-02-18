using System.Linq;
using System.Threading.Tasks;
using DotNetCoreWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreWebApi.Controllers.InvestmentAgreements
{
    [Route("api/agreements")]
    public class InvestmentAgreementsController : ControllerBase
    {
        private readonly DatabaseContext db;

        public InvestmentAgreementsController(DatabaseContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await db.DbInvestmentAgreements.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery]int id)
        {
            var agreement = await db.DbInvestmentAgreements.Where(a => a.Id == id).SingleAsync();
            return Ok(agreement);
        }

        [HttpPost("{profileId}")]
        public async Task Post([FromQuery] int profileId)
        {
            var profile = await db.DbProfiles.Where(p => p.Id == profileId).SingleAsync();
            var product = await db.DbProducts
                  .Where(p => p.Id == 1) // have a hardcoded value for product depending on some conditions
                  .SingleAsync();

            var agreement = new DbInvestmentAgreement
            {
                Product = product
            };

            db.DbInvestmentAgreements.Add(agreement);
            
            profile.InvestmentAgreements.Add(agreement);
            
            await db.SaveChangesAsync();
        }
    }
}