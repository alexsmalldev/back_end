using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; // Import for PasswordHasher
using TimeSheetApplicaiton.Models;
using TimeSheetApplication.Data;

namespace Time.Sheet.Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            // Generally, GET requests shouldn't create resources or return forms in APIs
            // This endpoint can be used to return any necessary metadata for user creation or removed if not needed
            return Ok("Provide user details for creation via POST.");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Email = userModel.Email
                };
                _context.Employees.Add(employee);
                _context.SaveChanges();

                var user = new User
                {
                    UserName = userModel.Email,
                    Password = _passwordHasher.HashPassword(null, userModel.Password),
                    EmployeeId = employee.EmployeeId
                };
                _context.Users.Add(user);
                _context.SaveChanges();

                return CreatedAtAction(nameof(Create), new { id = user.EmployeeId }, user); // Return a status code 201 and user info
            }

            return BadRequest(ModelState); // If ModelState is not valid, return a bad request with validation errors
        }
    }
}
