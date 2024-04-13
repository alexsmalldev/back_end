using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheetApplicaiton.Models;
using TimeSheetApplication.Data;

namespace Time.Sheet.Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRole(1)]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Index()
        {
            List<Employee> employees = await _context.Employees.ToListAsync();
            return Ok(employees); // Returns a 200 OK with the list of employees
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            // Typically, GET for create isn't used in APIs as creation is done via POST
            return BadRequest("Use POST to create a new entry.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Employee>> Create([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = employee.EmployeeId }, employee);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Details(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
        {
            if (id != employee.EmployeeId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent(); // Returns a 204 No Content on success
        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent(); // No content to signify successful delete
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}