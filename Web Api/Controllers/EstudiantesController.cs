using Web_Api.Contexts;
using Web_Api.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController : ControllerBase
{
    private readonly DbContextSQLSRV _context;

    public EstudiantesController(DbContextSQLSRV context) => _context = context;

    // GET: api/Estudiantes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        => await _context.Estudiantes.ToListAsync();

    // GET: api/Estudiantes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);
        return estudiante is null ? NotFound() : estudiante;
    }

    // POST: api/Estudiantes
    [HttpPost]
    public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
    {
        _context.Estudiantes.Add(estudiante);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.EstudianteId }, estudiante);
    }

    // PUT: api/Estudiantes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
    {
        if (id != estudiante.EstudianteId) return BadRequest("El id no coincide.");

        _context.Entry(estudiante).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Estudiantes.AnyAsync(e => e.EstudianteId == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    // DELETE: api/Estudiantes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEstudiante(int id)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);
        if (estudiante is null) return NotFound();

        if (await _context.Asignaciones.AnyAsync(a => a.EstudianteId == id))
            return Conflict("No se puede eliminar: el estudiante tiene asignaciones.");

        _context.Estudiantes.Remove(estudiante);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
