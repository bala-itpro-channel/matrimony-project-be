using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matrimony.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;

namespace Matrimony.Backend.Controllers
{
    [Authorize]
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepo;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            ILogger<ProfileController> logger,
            IProfileRepository profileRepo)
        {
            _logger = logger;
            _profileRepo = profileRepo;
        }

        // Action methods
        [HttpGet]
        public IEnumerable<Profile> Get()
        {
            try
            {
                return _profileRepo.GetAllProfile();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retriving profile list", ex);
                throw new Exception("Server Error", ex);
            }

        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // _logger.LogInformation($"New profile added - {profile.FullName}");

            return Ok(_profileRepo.Add(profile));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Profile profile)
        {
            if (profile == null)
            {
                return BadRequest();
            }

            // _db.Profiles.Remove(profile);
            // _db.Entry(profile).State = EntityState.Modified;  //Modify everything except id
            _profileRepo.Update(profile);

            return Ok("Update the profile successfully");
        }

        [Route("{id}")]
        public async Task<IActionResult> Delete(int id = 0)
        {
            // Profile profile = null;

            if (!(id > 0))
            {
                return BadRequest();
            }
            /*
            try
            {
                profile = _db.Profiles.First(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete failed, id not found", ex);
                return NotFound();
            }
            */

            // _db.Profiles.Remove(profile);
            // _db.Entry(profile).State = EntityState.Deleted;

            // await _db.SaveChangesAsync();

            _profileRepo.Delete(id);

            return Ok("Deleted the profile successfully");
        }

    }
}