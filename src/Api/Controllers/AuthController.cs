using Application.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
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
            string displayName = "";
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
                var user = await authService.LoginAsync(email, password);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    // GET: /HelloWorld/Welcome/ 
// Requires using System.Text.Encodings.Web;
public string Welcome(string name, int numTimes = 1)
{
    return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
}

https://localhost:{PORT}/HelloWorld/Welcome?name=Rick&numtimes=4.
    The URL segment Parameters isn't used.
    The name and numTimes parameters are passed in the query string.
    The ? (question mark) in the above URL is a separator, and the query string follows.
    The & character separates field-value pairs.

Run the app and enter the following URL: https://localhost:{PORT}/HelloWorld/Welcome/3?name=Rick

In the preceding URL:

    The third URL segment matched the route parameter id.
    The Welcome method contains a parameter id that matched the URL template in the MapControllerRoute method.
    The trailing ? starts the query string.

     // 
    // GET: /HelloWorld/
    public string Index()
    {
        return "This is my default action...";
    }
    // 
    // GET: /HelloWorld/Welcome/ 
    public string Welcome()
    {
        return "This is the Welcome action method...";
    }

*/