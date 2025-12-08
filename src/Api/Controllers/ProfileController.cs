using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        public ProfileController(ProfileService profileService)
        {
            this.profileService = profileService;
        }
        private readonly ProfileService profileService;
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var id = User.FindFirst("id")?.Value;
            if(id == null)
            {
                return BadRequest("User id not found in token.");
            }
            try
            {
                var profile = await profileService.GetProfileAsync(id!);
                return Ok(new { profile });

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("stats")]
        [Authorize]
        public async Task<IActionResult> Stats()
        {
            var id = User.FindFirst("id")?.Value;
            try
            {
                var stats = await profileService.GetStatsAsync(id!);
                return Ok(new { stats });

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}