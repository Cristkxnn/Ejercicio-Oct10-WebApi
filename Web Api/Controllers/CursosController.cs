using Web_Api.Contexts;
using Web_Api.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CursosController : ControllerBase
{
    private readonly DbContextSQLSRV _context;

    public CursosController(DbContextSQLSRV context) => _context = context;

    // GET: api/Cursos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        => await _context.Cursos.ToListAsync();

    // GET: api/Cursos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Curso>> GetCurso(int id)
    {
        var curso = await _context.Cursos.FindAsync(id);
        return curso is null ? NotFound() : curso;
    }

    // POST: api/Cursos
    [HttpPost]
    public async Task<ActionResult<Curso>> PostCurso(Curso curso)
    {
        _context.Cursos.Add(curso);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCurso), new { id = curso.CursoId }, curso);
    }

    // PUT: api/Cursos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCurso(int id, Curso curso)
    {
        if (id != curso.CursoId) return BadRequest("El id no coincide.");

        _context.Entry(curso).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Cursos.AnyAsync(e => e.CursoId == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    // DELETE: api/Cursos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCurso(int id)
    {
        var curso = await _context.Cursos.FindAsync(id);
        if (curso is null) return NotFound();

        if (await _context.Asignaciones.AnyAsync(a => a.CursoId == id))
            return Conflict("No se puede eliminar: el curso tiene asignaciones.");

        _context.Cursos.Remove(curso);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
