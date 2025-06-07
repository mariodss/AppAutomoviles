using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaBackend.Api.Data;
using PruebaBackend.Api.Models;

namespace PruebaBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasAutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MarcasAutosController(AppDbContext context)
        {
            _context = context;
        }

    
        /// Obtiene todas las marcas de autos.
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaAuto>>> GetMarcasAutos()
        {
            var marcas = await _context.MarcasAutos.ToListAsync();
            return Ok(marcas);
        }

        /// Agrega una nueva marca de auto.
    
        [HttpPost]
        public async Task<ActionResult<MarcaAuto>> PostMarcaAuto(MarcaAuto marca)
        {
            if (string.IsNullOrWhiteSpace(marca.Nombre))
                return BadRequest("El nombre es obligatorio.");

            _context.MarcasAutos.Add(marca);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMarcasAutos), new { id = marca.Id }, marca);
        }

        
        /// Actualiza una marca de auto existente.
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarcaAuto(int id, MarcaAuto marca)
        {
            if (id != marca.Id)
                return BadRequest("El id no coincide.");

            var existe = await _context.MarcasAutos.AnyAsync(m => m.Id == id);
            if (!existe)
                return NotFound();

            _context.Entry(marca).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

    
        /// Elimina una marca de auto.
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarcaAuto(int id)
        {
            var marca = await _context.MarcasAutos.FindAsync(id);
            if (marca == null)
                return NotFound();

            _context.MarcasAutos.Remove(marca);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}