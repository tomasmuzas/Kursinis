using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using MigrationDemo.Entities;

namespace MigrationDemo.Controllers.InvestmentAgreements
{
    [Route("api/agreements")]
    public class InvestmentAgreementsController : ApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();
        
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await db.InvestmentAgreements.ToListAsync());
        }

        [Route("api/agreements/{id}")]
        public async Task<IHttpActionResult> Get([FromUri]int id)
        {
            var agreement = await db.InvestmentAgreements.Where(a => a.Id == id).SingleAsync();
            return Ok(agreement);
        }

        [Route("api/agreements/{profileId}")]
        public async Task Post([FromBody] AgreementDataJson data, [FromUri] int profileId)
        {
            var profile = await db.Profiles.Where(p => p.Id == profileId).SingleAsync();

            var agreement = new DbInvestmentAgreement();

            db.InvestmentAgreements.Add(agreement);
            
            profile.InvestmentAgreements.Add(agreement);
            
            await db.SaveChangesAsync();
        }
    }
}
