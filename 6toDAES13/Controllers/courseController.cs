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
    public class courseController : ControllerBase
    {
        private readonly ConectDB _context;

        public courseController(ConectDB context)
        {
            _context = context;
        }
        // GET: api/course/Listar
        [HttpGet(Name = "ListarCursos")]
        public async Task<ActionResult<IEnumerable<Course>>> ListarCursos()
        {
            return await _context.Courses.Where(g => g.Active).ToListAsync();
        }
        // GET: api/course/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> Details(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null || !course.Active)
            {
                return NotFound();
            }

            return course;
        }
        // POST: api/course/Create
        [HttpPost]
        public async Task<ActionResult<Course>> Create(Course course)
        {
            course.Active = true;
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = course.CourseID }, course);
        }
        // PUT: api/course/Update/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Course course)
        {
            if (id != course.CourseID)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // DELETE: api/course/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null || !course.Active)
            {
                return NotFound();
            }

            course.Active = false;
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }

    }
}
