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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            string email = request.Email;
            string password = request.Password;
            try
            {
                var authResponse = await authService.LoginAsync(email, password);
                return Ok(authResponse);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestDto request)
        {
            string refreshToken = request.RefreshToken;
            if(refreshToken == null)
            {
                return BadRequest("Refresh token is required.");
            }
            try
            {
                var token = await authService.RefreshTokenAsync(refreshToken);
                return Ok(token);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}