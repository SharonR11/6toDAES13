using _6toDAES13.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6toDAES13.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class studentController : ControllerBase
    {
        private readonly ConectDB _context;

        public studentController(ConectDB context)
        {
            _context = context;
        }

        // GET: api/student/Listar
        [HttpGet(Name = "ListarEstudiantes")]
        public async Task<ActionResult<IEnumerable<Student>>> ListarEstudiantes()
        {
            return await _context.Students.Where(s => s.Active).ToListAsync();
        }
        // GET: api/student/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Details(int id)
        {
            var student = await _context.Students.Include(s => s.Grade).FirstOrDefaultAsync(s => s.StudentID == id && s.Active);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // POST: api/student/Create
        [HttpPost]
        public async Task<ActionResult<Student>> Create(Student student)
        {
            student.Active = true;
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = student.StudentID }, student);
        }

        // PUT: api/student/Update/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // DELETE: api/student/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null || !student.Active)
            {
                return NotFound();
            }

            student.Active = false;
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }

        // GET: api/student/BuscarPorNombreApellidoEmail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> BuscarPorNombreApellidoEmail(string nombre, string apellido, string email)
        {
            var students = _context.Students.Where(s =>
                (string.IsNullOrEmpty(nombre) || s.FirstName.Contains(nombre)) &&
                (string.IsNullOrEmpty(apellido) || s.LastName.Contains(apellido)) &&
                (string.IsNullOrEmpty(email) || s.Email.Contains(email))
            ).OrderByDescending(s => s.LastName);

            return await students.ToListAsync();
        }

        // GET: api/student/BuscarPorNombreGrado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> BuscarPorNombreGrado(string nombre, int grado)
        {
            var students = _context.Students.Include(s => s.Grade)
                .Where(s =>
                    (string.IsNullOrEmpty(nombre) || s.FirstName.Contains(nombre)) &&
                    s.GradeID == grado
                ).OrderByDescending(s => s.Grade.Name);

            return await students.ToListAsync();
        }

    }
}
