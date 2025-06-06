using Microsoft.EntityFrameworkCore;
using PruebaBackend.Api.Models;

namespace PruebaBackend.Api.Data
{
    
    /// DbContext principal para la aplicación.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<MarcaAuto> MarcasAutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed de datos iniciales
            modelBuilder.Entity<MarcaAuto>().HasData(
                new MarcaAuto { Id = 1, Nombre = "Toyota" },
                new MarcaAuto { Id = 2, Nombre = "Ford" },
                new MarcaAuto { Id = 3, Nombre = "Volkswagen" }
            );
        }
    }
}