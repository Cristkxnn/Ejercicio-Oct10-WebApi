using Web_Api.Contexts;
using Web_Api.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocentesController : ControllerBase
{
    private readonly DbContextSQLSRV _context;

    public DocentesController(DbContextSQLSRV context) => _context = context;

    // GET: api/Docentes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Docente>>> GetDocentes()
        => await _context.Docentes.ToListAsync();

    // GET: api/Docentes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Docente>> GetDocente(int id)
    {
        var docente = await _context.Docentes.FindAsync(id);
        return docente is null ? NotFound() : docente;
    }

    // POST: api/Docentes
    [HttpPost]
    public async Task<ActionResult<Docente>> PostDocente(Docente docente)
    {
        _context.Docentes.Add(docente);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDocente), new { id = docente.DocenteId }, docente);
    }

    // PUT: api/Docentes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDocente(int id, Docente docente)
    {
        if (id != docente.DocenteId) return BadRequest("El id no coincide.");

        _context.Entry(docente).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Docentes.AnyAsync(e => e.DocenteId == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    // DELETE: api/Docentes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocente(int id)
    {
        var docente = await _context.Docentes.FindAsync(id);
        if (docente is null) return NotFound();

        if (await _context.Asignaciones.AnyAsync(a => a.DocenteId == id))
            return Conflict("No se puede eliminar: el docente tiene asignaciones.");

        _context.Docentes.Remove(docente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
