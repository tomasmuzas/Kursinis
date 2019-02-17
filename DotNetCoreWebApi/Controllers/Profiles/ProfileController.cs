using System.Threading.Tasks;
using DotNetCoreWebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreWebApi.Controllers.Profiles
{
    [Route("api/profiles")]
    public class ProfileController : ControllerBase
    {
        private readonly DatabaseContext db = new DatabaseContext();
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await db.Profiles.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery]int id)
        {
            var profile = await db.Profiles.Where(p => p.Id == id).SingleAsync();
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

            db.Profiles.Add(profile);

            await db.SaveChangesAsync();
        }
    }
}
