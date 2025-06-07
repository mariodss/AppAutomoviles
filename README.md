# Prueba de Conocimiento Backend en C#


## Tecnologías utilizadas

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- XUnit
- Docker & Docker Compose

## Configuración y ejecución

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd PruebaBackend
```

### 2. Variables de entorno

La cadena de conexión a la base de datos se configura automáticamente en Docker Compose.  
Si corres localmente, revisa `appsettings.json` en `PruebaBackend.Api`.


### 3. Levantar el entorno con Docker Compose

Asegúrate de tener Docker Desktop instalado y corriendo.

```bash
docker-compose up --build
```

- La API estará disponible en: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- PostgreSQL estará en el puerto 5432.



### 4. Ejecutar pruebas unitarias

```bash
dotnet test
```

#### Para ver el reporte de cobertura:

```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:PruebaBackend.Tests/TestResults/**/coverage.cobertura.xml -targetdir:coveragereport
```

Abre `coveragereport/index.html` en tu navegador.

## Endpoints principales

- `GET /api/MarcasAutos` - Lista todas las marcas de autos.
- `POST /api/MarcasAutos` - Agrega una nueva marca.
- `PUT /api/MarcasAutos/{id}` - Actualiza una marca existente.
- `DELETE /api/MarcasAutos/{id}` - Elimina una marca.

## Notas

- El proyecto incluye migraciones y seed de datos para la tabla `MarcasAutos`.
- Las pruebas unitarias cubren los principales casos de uso y errores.
- El entorno es portable y reproducible gracias a Docker Compose.

---
