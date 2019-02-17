using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using MigrationDemo.Entities;

namespace MigrationDemo.Controllers.Profiles
{
    [Route("api/profiles")]
    public class ProfileController : ApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();
        
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await db.Profiles.ToListAsync());
        }

        [Route("api/profiles/{id}")]
        public async Task<IHttpActionResult> Get([FromUri]int id)
        {
            var profile = await db.Profiles.Where(p => p.Id == id).SingleAsync();
            return Ok(profile);
        }
        
        public async Task Post([FromBody] ProfileDataJson data)
        {
            var profile = new DbProfile
            {
                Name = data.Name,
                Surname = data.Surname,
                Email = data.Email
            };

            db.Profiles.Add(profile);

            await db.SaveChangesAsync();
        }
    }
}
