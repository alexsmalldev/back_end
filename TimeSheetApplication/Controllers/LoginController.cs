using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TimeSheetApplicaiton.Models;
using TimeSheetApplication.Data;

namespace Time.Sheet.Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            // For APIs, you typically wouldn't return a view for login. You might return a status or a specific message.
            return Ok("Login endpoint hit. Use POST to submit credentials.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await AuthenticateUser(model.Email, model.Password);
                if (user != null)
                {
                    var role = await _context.Roles.FindAsync(user.RoleId);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("RoleId", user.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, role.RoleShortDesc)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Ok(new { message = "Login successful" });
                }
                else
                {
                    return Unauthorized("Invalid login attempt.");
                }
            }
            return BadRequest("Invalid data");
        }

        private async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == email);
            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user; // Authentication successful
                }
            }
            return null; // Authentication failed
        }
    }
}
