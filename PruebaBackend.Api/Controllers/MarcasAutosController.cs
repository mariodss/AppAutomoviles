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

        /// <summary>
        /// Obtiene todas las marcas de autos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaAuto>>> GetMarcasAutos()
        {
            var marcas = await _context.MarcasAutos.ToListAsync();
            return Ok(marcas);
        }

        /// <summary>
        /// Agrega una nueva marca de auto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MarcaAuto>> PostMarcaAuto(MarcaAuto marca)
        {
            if (string.IsNullOrWhiteSpace(marca.Nombre))
                return BadRequest("El nombre es obligatorio.");

            _context.MarcasAutos.Add(marca);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMarcasAutos), new { id = marca.Id }, marca);
        }
    }
}