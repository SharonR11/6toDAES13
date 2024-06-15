using _6toDAES13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _6toDAES13.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class gradeController : ControllerBase
    {
        private readonly ConectDB _context;

        public gradeController(ConectDB context)
        {
            _context = context;
        }

        // GET: api/grade/Listar
        [HttpGet(Name = "ListarGrados")]
        public async Task<ActionResult<IEnumerable<Grade>>> ListarGrados()
        {
            return await _context.Grades.Where(g => g.Active).ToListAsync();
        }

        // GET: api/grade/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> Details(int id)
        {
            var grade = await _context.Grades.FindAsync(id);

            if (grade == null || !grade.Active)
            {
                return NotFound();
            }

            return grade;
        }

        // POST: api/grade/Create
        [HttpPost]
        public async Task<ActionResult<Grade>> Create(Grade grade)
        {
            grade.Active = true;
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = grade.GradeID }, grade);
        }

        // PUT: api/grade/Update/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Grade grade)
        {
            if (id != grade.GradeID)
            {
                return BadRequest();
            }

            _context.Entry(grade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/grade/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null || !grade.Active)
            {
                return NotFound();
            }

            grade.Active = false;
            _context.Entry(grade).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.GradeID == id);
        }
    }
}
