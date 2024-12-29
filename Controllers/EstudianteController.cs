using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD; //

        public EstudianteController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;//
        }
        [HttpGet]
        [Route("GetTodosLosEstudiantes")]
        public async Task<IActionResult> GetTodosLosEstudiantes()
        {
            var lista = await appCnxinBD.Estudiante
                .FromSqlInterpolated($"CALL LeerEstudiantes()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetEstudiantePorID/{id}")]
        public async Task<IActionResult> GetEstudiantePorID(int id)
        {
            var estudiante = await appCnxinBD.Estudiante
                .FromSqlRaw("SELECT * FROM estudiante WHERE IDEstudiante= {0}", id)
                .FirstOrDefaultAsync();

            if (estudiante == null)
            {
                return NotFound($"No se encontró un estudiante con el ID {id}");
            }

            return Ok(estudiante);
        }
        [HttpPost]
        [Route("CrearEstudiante")]
        public async Task<IActionResult> CrearEstudiante([FromBody] EstudianteModel nuevoEstudiante)
        {
            if (nuevoEstudiante == null)
            {
                return BadRequest("Los datos del estudiante son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearEstudiante({nuevoEstudiante.Nombre}, {nuevoEstudiante.Apellido}, {nuevoEstudiante.Grado}, {nuevoEstudiante.NivelDeIngles}, {nuevoEstudiante.IDNivel})");

            return Ok("Estudiante creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarEstudiante/{id}")]
        public async Task<IActionResult> ActualizarEstudiante(int id, [FromBody] EstudianteModel estudianteActualizado)
        {
            if (estudianteActualizado == null)
            {
                return BadRequest("Los datos del estudiante son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarEstudiante({id}, {estudianteActualizado.Nombre}, {estudianteActualizado.Apellido}, {estudianteActualizado.Grado}, {estudianteActualizado.NivelDeIngles}, {estudianteActualizado.IDNivel})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un estudiante con el ID {id} para actualizar.");
            }

            return Ok("Estudiante actualizado.");
        }
        [HttpDelete]
        [Route("EliminarEstudiante/{id}")]
        public async Task<IActionResult> EliminarEstudiante(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarEstudiante({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un estudiante con el ID {id} para eliminar.");
            }

            return Ok("Estudiante eliminado.");
        }

    }
}
