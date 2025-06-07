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

        //Verifica que el endpoint GET devuelve exactamente las 3 marcas de autos iniciales cuando la base de datos tiene datos.

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

        //Verifica que el endpoint GET devuelve una lista vacía cuando la base de datos no tiene marcas.

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

        //Verifica que el endpoint GET devuelve las marcas correctas ("Toyota", "Ford", "Volkswagen") en la respuesta.

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

        // Verifica que el endpoint POST agrega una nueva marca válida ("Honda") y la respuesta es la esperada.
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


        // Verifica que el endpoint POST devuelve BadRequest cuando se intenta agregar una marca con nombre vacío.
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

        // Verifica que el endpoint PUT actualiza correctamente el nombre de una marca existente.

        [Fact]
        public async Task PutMarcaAuto_UpdatesMarca_WhenValid()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new MarcasAutosController(context);
            var marca = context.MarcasAutos.First();
            marca.Nombre = "Actualizado";

            // Act
            var result = await controller.PutMarcaAuto(marca.Id, marca);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Actualizado", context.MarcasAutos.Find(marca.Id)?.Nombre);
        }

        // Verifica que el endpoint PUT devuelve NotFound si se intenta actualizar una marca que no existe.


        [Fact]
        public async Task PutMarcaAuto_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new MarcasAutosController(context);
            var marca = new MarcaAuto { Id = 999, Nombre = "NoExiste" };

            // Act
            var result = await controller.PutMarcaAuto(999, marca);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Verifica que el endpoint DELETE elimina correctamente una marca existente.

        [Fact]
        public async Task DeleteMarcaAuto_DeletesMarca_WhenExists()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new MarcasAutosController(context);
            var marca = context.MarcasAutos.First();

            // Act
            var result = await controller.DeleteMarcaAuto(marca.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.MarcasAutos.Find(marca.Id));
        }

        // Verifica que el endpoint DELETE devuelve NotFound si se intenta eliminar una marca que no existe.
        [Fact]
        public async Task DeleteMarcaAuto_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new MarcasAutosController(context);

            // Act
            var result = await controller.DeleteMarcaAuto(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}