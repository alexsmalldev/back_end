using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetApplicaiton.Models;
using TimeSheetApplication.Data;

namespace Time.Sheet.Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRole(1)]
    public class JobController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> Index()
        {
            var jobsWithContractors = await _context.Jobs.Include(j => j.Contractor).ToListAsync();
            return Ok(jobsWithContractors);
        }

        [HttpGet("create")]
        public async Task<ActionResult<IEnumerable<Contractor>>> Create()
        {
            var contractors = await _context.Contractors.ToListAsync();
            return Ok(contractors); // Send the list of contractors to the client for selection
        }

        [HttpPost]
        public async Task<ActionResult<Job>> Create([FromBody] Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = job.JobId }, job);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> Details(int id)
        {
            var job = await _context.Jobs.Include(j => j.Contractor).FirstOrDefaultAsync(j => j.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DetailsUpdate(int id, [FromBody] Job job)
        {
            if (id != job.JobId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Update(job);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Jobs.Any(j => j.JobId == id))
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
