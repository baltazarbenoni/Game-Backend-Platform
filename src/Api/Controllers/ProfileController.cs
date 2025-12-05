using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        public ProfileController(ProfileService profileService)
        {
            this.profileService = profileService;
        }
        private readonly ProfileService profileService;
        public async Task<IActionResult> Profile()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var displayName = await profileService.GetProfileAsync(id!);
                return Ok(new { displayName });

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}