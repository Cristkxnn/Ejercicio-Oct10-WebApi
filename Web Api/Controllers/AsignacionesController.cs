using Web_Api.Contexts;
using Web_Api.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AsignacionesController : ControllerBase
{
    private readonly DbContextSQLSRV _context;

    public AsignacionesController(DbContextSQLSRV context) => _context = context;

    // GET: api/Asignaciones  (incluye Estudiante, Curso y Docente)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asignacion>>> GetAsignaciones()
        => await _context.Asignaciones
            .Include(a => a.Estudiante)
            .Include(a => a.Curso)
            .Include(a => a.Docente)
            .ToListAsync();

    // GET: api/Asignaciones/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Asignacion>> GetAsignacion(int id)
    {
        var asignacion = await _context.Asignaciones
            .Include(a => a.Estudiante)
            .Include(a => a.Curso)
            .Include(a => a.Docente)
            .FirstOrDefaultAsync(a => a.AsignacionId == id);

        return asignacion is null ? NotFound() : asignacion;
    }

    // POST: api/Asignaciones
    // Body: { "estudianteId": 1, "cursoId": 1, "docenteId": 1 }
    [HttpPost]
    public async Task<ActionResult<Asignacion>> PostAsignacion(Asignacion asignacion)
    {
        var error = await ValidarLlaves(asignacion);
        if (error is not null) return BadRequest(error);

        // Evita que EF intente insertar objetos de navegación enviados en el body
        asignacion.Estudiante = null;
        asignacion.Curso = null;
        asignacion.Docente = null;

        _context.Asignaciones.Add(asignacion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAsignacion), new { id = asignacion.AsignacionId }, asignacion);
    }

    // PUT: api/Asignaciones/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsignacion(int id, Asignacion asignacion)
    {
        if (id != asignacion.AsignacionId) return BadRequest("El id no coincide.");

        var existente = await _context.Asignaciones.FindAsync(id);
        if (existente is null) return NotFound();

        var error = await ValidarLlaves(asignacion);
        if (error is not null) return BadRequest(error);

        existente.EstudianteId = asignacion.EstudianteId;
        existente.CursoId = asignacion.CursoId;
        existente.DocenteId = asignacion.DocenteId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Asignaciones/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsignacion(int id)
    {
        var asignacion = await _context.Asignaciones.FindAsync(id);
        if (asignacion is null) return NotFound();

        _context.Asignaciones.Remove(asignacion);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Verifica que existan el estudiante, el curso y el docente
    private async Task<string?> ValidarLlaves(Asignacion a)
    {
        if (!await _context.Estudiantes.AnyAsync(e => e.EstudianteId == a.EstudianteId))
            return $"No existe el estudiante con id {a.EstudianteId}.";
        if (!await _context.Cursos.AnyAsync(c => c.CursoId == a.CursoId))
            return $"No existe el curso con id {a.CursoId}.";
        if (!await _context.Docentes.AnyAsync(d => d.DocenteId == a.DocenteId))
            return $"No existe el docente con id {a.DocenteId}.";
        return null;
    }
}
