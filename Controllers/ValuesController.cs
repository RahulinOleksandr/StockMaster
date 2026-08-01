using Stock_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Stock_Master.Data;
using Microsoft.EntityFrameworkCore;


namespace Stock_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        private readonly DBContent _context;

        public ValuesController(DBContent context)
        {
            _context = context;
        }

        // GET "GetJobList"
        [HttpGet("GetJobList")]
        public async Task<ActionResult<List<Class>>> GetAllInfo()
        {
            return await _context.Jobs.ToListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetById(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();
            return job;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Class>> Create(Class newJob)
        {
            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newJob.jobId }, newJob);
        }

        // PUT "5"
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Class updatedJob)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            job.jobName = updatedJob.jobName;
            job.jobDescription = updatedJob.jobDescription;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE "5"
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
