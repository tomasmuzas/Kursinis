using System.Linq;
using System.Threading.Tasks;
using DotNetCoreWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreWebApi.Controllers.Profiles
{
    [Route("api/profiles")]
    public class ProfileController : ControllerBase
    {
        private readonly DatabaseContext db;

        public ProfileController(DatabaseContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await db.DbProfiles.Include(p => p.InvestmentAgreements).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery]int id)
        {
            var profile = await db.DbProfiles.Where(p => p.Id == id).SingleAsync();
            return Ok(profile);
        }
        
        [HttpPost]
        public async Task Post([FromBody] ProfileDataJson data)
        {
            var profile = new DbProfile
            {
                Name = data.Name,
                Surname = data.Surname,
                Email = data.Email
            };

            db.DbProfiles.Add(profile);

            await db.SaveChangesAsync();
        }
    }
}
