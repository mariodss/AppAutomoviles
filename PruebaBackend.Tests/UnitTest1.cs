using Xunit;
using PruebaBackend.Api.Controllers;
using PruebaBackend.Api.Data;
using PruebaBackend.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PruebaBackend.Tests
{
    public class MarcasAutosControllerTests
    {
        private AppDbContext GetDbContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            if (withData)
            {
                context.MarcasAutos.AddRange(
                    new MarcaAuto { Id = 1, Nombre = "Toyota" },
                    new MarcaAuto { Id = 2, Nombre = "Ford" },
                    new MarcaAuto { Id = 3, Nombre = "Volkswagen" }
                );
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public async Task GetMarcasAutos_ReturnsAllMarcas()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new MarcasAutosController(context);

            // Act
            var result = await controller.GetMarcasAutos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var marcas = Assert.IsAssignableFrom<IEnumerable<MarcaAuto>>(okResult.Value);
            Assert.Equal(3, marcas.Count());
        }

        [Fact]
        public async Task GetMarcasAutos_ReturnsEmptyList_WhenNoData()
        {
            // Arrange
            var context = GetDbContext(withData: false);
            var controller = new MarcasAutosController(context);

            // Act
            var result = await controller.GetMarcasAutos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var marcas = Assert.IsAssignableFrom<IEnumerable<MarcaAuto>>(okResult.Value);
            Assert.Empty(marcas);
        }

        [Fact]
        public async Task GetMarcasAutos_ReturnsCorrectData()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new MarcasAutosController(context);

            // Act
            var result = await controller.GetMarcasAutos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var marcas = Assert.IsAssignableFrom<IEnumerable<MarcaAuto>>(okResult.Value);
            Assert.Contains(marcas, m => m.Nombre == "Toyota");
            Assert.Contains(marcas, m => m.Nombre == "Ford");
            Assert.Contains(marcas, m => m.Nombre == "Volkswagen");
        }

        [Fact]
        public async Task PostMarcaAuto_AddsMarca_WhenValid()
        {
            // Arrange
            var context = GetDbContext(withData: false);
            var controller = new MarcasAutosController(context);
            var nuevaMarca = new MarcaAuto { Nombre = "Honda" };

            // Act
            var result = await controller.PostMarcaAuto(nuevaMarca);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var marca = Assert.IsType<MarcaAuto>(createdResult.Value);
            Assert.Equal("Honda", marca.Nombre);
            Assert.Single(context.MarcasAutos);
        }

        [Fact]
        public async Task PostMarcaAuto_ReturnsBadRequest_WhenNombreIsEmpty()
        {
            // Arrange
            var context = GetDbContext(withData: false);
            var controller = new MarcasAutosController(context);
            var nuevaMarca = new MarcaAuto { Nombre = "" };

            // Act
            var result = await controller.PostMarcaAuto(nuevaMarca);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}