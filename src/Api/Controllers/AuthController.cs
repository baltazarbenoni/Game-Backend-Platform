using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }
        private readonly AuthService authService;
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            string email = request.Email;
            string password = request.Password;
            string displayName = request.DisplayName;
            try
            {
                await authService.RegisterAsync(email, password, displayName);
                return Ok("User registered successfully.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            string email = request.Email;
            string password = request.Password;
            try
            {
                var token = await authService.LoginAsync(email, password);
                return Ok(new { token });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}