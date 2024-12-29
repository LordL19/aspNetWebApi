using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;
        public ProfesorController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosProfesores")]
        public async Task<IActionResult> GetTodosLosProfesores()
        {
            var lista = await appCnxinBD.Profesor
                .FromSqlInterpolated($"CALL LeerProfesores()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetProfesorPorID/{id}")]
        public async Task<IActionResult> GetProfesorPorID(int id)
        {
            var profesor = await appCnxinBD.Profesor
                .FromSqlRaw("SELECT * FROM profesor WHERE IDProfesor = {0}",id)
                .FirstOrDefaultAsync();

            if (profesor == null)
            {
                return NotFound($"No se encontró un profesor con el ID {id}");
            }

            return Ok(profesor);
        }
        [HttpPost]
        [Route("CrearProfesor")]
        public async Task<IActionResult> CrearProfesor([FromBody] ProfesorModel nuevoProfesor)
        {
            if (nuevoProfesor == null)
            {
                return BadRequest("Los datos del profesor son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearProfesor({nuevoProfesor.Nombre}, {nuevoProfesor.Apellido}, {nuevoProfesor.Especialidad}, {nuevoProfesor.IDAula}, {nuevoProfesor.IDNivel}, {nuevoProfesor.IDAsignaturaAsignada})");

            return Ok("Profesor creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarProfesor/{id}")]
        public async Task<IActionResult> ActualizarProfesor(int id, [FromBody] ProfesorModel profesorActualizado)
        {
            if (profesorActualizado == null)
            {
                return BadRequest("Los datos del profesor son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarProfesor({id}, {profesorActualizado.Nombre}, {profesorActualizado.Apellido}, {profesorActualizado.Especialidad}, {profesorActualizado.IDAula}, {profesorActualizado.IDNivel}, {profesorActualizado.IDAsignaturaAsignada})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un profesor con el ID {id} para actualizar.");
            }

            return Ok("Profesor actualizado.");
        }
        [HttpDelete]
        [Route("EliminarProfesor/{id}")]
        public async Task<IActionResult> EliminarProfesor(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarProfesor({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un profesor con el ID {id} para eliminar.");
            }

            return Ok("Profesor eliminado.");
        }
    }
}
