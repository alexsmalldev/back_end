using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheetApplicaiton.Models;
using TimeSheetApplication.Data;

namespace Time.Sheet.Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRole(1)] // Ensure your custom authorization attribute is compatible with API controllers
    public class ContractorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContractorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Contractor>>> Index()
        {
            List<Contractor> contractors = await _context.Contractors.ToListAsync();
            return Ok(contractors); // Returns a list of contractors with a status code of 200 OK
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return BadRequest("Use POST to create new entries.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Contractor>> Create([FromBody] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                _context.Contractors.Add(contractor);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = contractor.ContractorId }, contractor);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contractor>> Details(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return NotFound();
            }
            return Ok(contractor);
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsUpdate(int id, [FromBody] Contractor contractor)
        {
            if (id != contractor.ContractorId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Entry(contractor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Returns a 204 No Content on successful update
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return NotFound();
            }
            _context.Contractors.Remove(contractor);
            await _context.SaveChangesAsync();
            return NoContent(); // Returns a 204 No Content on successful deletion
        }

        private bool ContractorExists(int id)
        {
            return _context.Contractors.Any(e => e.ContractorId == id);
        }
    }
}
