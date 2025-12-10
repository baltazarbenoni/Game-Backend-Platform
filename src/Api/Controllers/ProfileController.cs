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
            var profile = await profileService.GetProfileAsync(id!);
            return Ok(new { profile });
        }
        [HttpGet("stats")]
        [Authorize]
        public async Task<IActionResult> Stats()
        {
            var id = User.FindFirst("id")?.Value;
            var stats = await profileService.GetStatsAsync(id!);
            return Ok(new { stats });
        }
    }

}