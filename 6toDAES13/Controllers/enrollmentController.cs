using _6toDAES13.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6toDAES13.Controllers
{
    [Route("api/[controller]/[action]")]

    [ApiController]
    public class enrollmentController : ControllerBase
    {
        private readonly ConectDB _context;

        public enrollmentController(ConectDB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> Listar()
        {
            return await _context.Enrollments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> Detalle(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> Crear(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Detalle), new { id = enrollment.EnrollmentID }, enrollment);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Actualizar(int id, Enrollment enrollment)
        //{
        //    if (id != enrollment.EnrollmentID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(enrollment).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EnrollmentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Eliminar(int id)
        //{
        //    var enrollment = await _context.Enrollments.FindAsync(id);
        //    if (enrollment == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Enrollments.Remove(enrollment);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> BuscarPorNombreCurso(string nombreCurso)
        {
            var enrollments = _context.Enrollments.Include(e => e.Student).Include(e => e.Course)
                .Where(e => e.Course.Name.Contains(nombreCurso))
                .OrderBy(e => e.Course.Name).ThenBy(e => e.Student.LastName);

            return await enrollments.ToListAsync();
        }

        [HttpGet]
        public IEnumerable<Enrollment> BuscarPorGrado(string gradoNombre)
        {
            var enrollments = _context.Enrollments.Include(e => e.Student).Include(e => e.Course)
            .Where(e => e.Student.Grade != null && e.Student.Grade.Name.Contains(gradoNombre))
            .OrderBy(e => e.Course.Name).ThenBy(e => e.Student.LastName);

            return  enrollments.ToList();
        }

    }
}
