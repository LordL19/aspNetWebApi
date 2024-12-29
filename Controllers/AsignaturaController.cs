using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignaturaController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public AsignaturaController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }

        [HttpGet]
        [Route("GetTodasLasAsignaturas")]
        public async Task<IActionResult> GetTodasLasAsignaturas()
        {
            var lista_de_asignaturas = await appCnxinBD.Asignatura
                .FromSqlInterpolated($"CALL LeerAsignaturas()")
                .ToListAsync();

            return Ok(lista_de_asignaturas);
        }

        [HttpGet]
        [Route("GetAsignaturaPorID/{id}")]
        public async Task<IActionResult> GetAsignaturaPorID(int id)
        {
            var asignatura = await appCnxinBD.Asignatura
                .FromSqlRaw("SELECT * FROM asignatura WHERE IDAsignatura = {0}", id)
                .FirstOrDefaultAsync();

            if (asignatura == null)
            {
                return NotFound($"No se encontró una asignatura con el ID {id}");
            }

            return Ok(asignatura);
        }
        [HttpPost]
        [Route("CrearAsignatura")]
        public async Task<IActionResult> CrearAsignatura([FromBody] AsignaturaModel nuevaAsignatura)
        {
            if (nuevaAsignatura == null)
            {
                return BadRequest("Los datos de la asignatura son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearAsignatura({nuevaAsignatura.Nombre}, {nuevaAsignatura.IDAula})");

            return Ok("Asignatura creada exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarAsignatura/{id}")]
        public async Task<IActionResult> ActualizarAsignatura(int id, [FromBody] AsignaturaModel asignaturaActualizada)
        {
            if (asignaturaActualizada == null)
            {
                return BadRequest("Los datos de la asignatura son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarAsignatura({id}, {asignaturaActualizada.Nombre}, {asignaturaActualizada.IDAula})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una asignatura con el ID {id} para actualizar.");
            }

            return Ok("Asignatura actualizada.");
        }
        [HttpDelete]
        [Route("EliminarAsignatura/{id}")]
        public async Task<IActionResult> EliminarAsignatura(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarAsignatura({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una asignatura con el ID {id} para eliminar.");
            }

            return Ok("Asignatura eliminada.");
        }
    }
}
