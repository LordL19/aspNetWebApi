using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignaturaAsignadaController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public AsignaturaAsignadaController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }

        [HttpGet]
        [Route("GetTodasLasAsignaturasAsignadas")]
        public async Task<IActionResult> GetTodasLasAsignaturasAsignadas()
        {
            var lista = await appCnxinBD.AsignaturaAsignada
                .FromSqlInterpolated($"CALL LeerAsignaturasAsignadas()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetAsignaturaAsignadaPorID/{id}")]
        public async Task<IActionResult> GetAsignaturaAsignadaPorID(int id)
        {
            var asignaturaAsignada = await appCnxinBD.AsignaturaAsignada
                .FromSqlRaw("SELECT * FROM asignaturaasignada WHERE IDAsignaturaAsignada = {0}", id)
                .FirstOrDefaultAsync();

            if (asignaturaAsignada == null)
            {
                return NotFound($"No se encontró una asignatura asignada con el ID {id}");
            }

            return Ok(asignaturaAsignada);
        }
        [HttpPost]
        [Route("CrearAsignaturaAsignada")]
        public async Task<IActionResult> CrearAsignaturaAsignada([FromBody] AsignaturaAsignadaModel nuevaAsignaturaAsignada)
        {
            if (nuevaAsignaturaAsignada == null)
            {
                return BadRequest("Los datos de la asignación son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearAsignaturaAsignada({nuevaAsignaturaAsignada.IDNivel}, {nuevaAsignaturaAsignada.IDProfesor}, {nuevaAsignaturaAsignada.IDAsignatura})");

            return Ok("Asignatura asignada creada exitosamente.");
        }

 
        [HttpPut]
        [Route("ActualizarAsignaturaAsignada/{id}")]
        public async Task<IActionResult> ActualizarAsignaturaAsignada(int id, [FromBody] AsignaturaAsignadaModel AsignaturaAsignadaActualizada)
        {
            if (AsignaturaAsignadaActualizada == null)
            {
                return BadRequest("Los datos de la asignación son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarAsignaturaAsignada({id}, {AsignaturaAsignadaActualizada.IDNivel}, {AsignaturaAsignadaActualizada.IDProfesor}, {AsignaturaAsignadaActualizada.IDAsignatura})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una asignatura asignada con el ID {id} para actualizar.");
            }

            return Ok("Asignatura asignada actualizada.");
        }
        [HttpDelete]
        [Route("EliminarAsignaturaAsignada/{id}")]
        public async Task<IActionResult> EliminarAsignaturaAsignada(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarAsignaturaAsignada({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una asignatura asignada con el ID {id} para eliminar.");
            }

            return Ok("Asignatura asignada eliminada.");
        }


    }
}
