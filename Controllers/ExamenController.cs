using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public ExamenController (AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosExamenes")]
        public async Task<IActionResult> GetTodosLosExamenes()
        {
            var lista = await appCnxinBD.Examen
                .FromSqlInterpolated($"CALL LeerExamenes()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetExamenPorID/{id}")]
        public async Task<IActionResult> GetExamenPorID(int id)
        {
            var examen = await appCnxinBD.Examen
                .FromSqlRaw("SELECT * FROM examen WHERE IDExamen = {0}", id)
                .FirstOrDefaultAsync();

            if (examen == null)
            {
                return NotFound($"No se encontró un examen con el ID {id}");
            }

            return Ok(examen);
        }
        [HttpPost]
        [Route("CrearExamen")]
        public async Task<IActionResult> CrearExamen([FromBody] ExamenModel nuevoExamen)
        {
            if (nuevoExamen == null)
            {
                return BadRequest("Los datos del examen son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearExamen({nuevoExamen.IDAsignatura}, {nuevoExamen.IDProfesor}, {nuevoExamen.FechaEvaluacion}, {nuevoExamen.TipoExamen})");

            return Ok("Examen creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarExamen/{id}")]
        public async Task<IActionResult> ActualizarExamen(int id, [FromBody] ExamenModel examenActualizado)
        {
            if (examenActualizado == null)
            {
                return BadRequest("Los datos del examen son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarExamen({id}, {examenActualizado.IDAsignatura}, {examenActualizado.IDProfesor}, {examenActualizado.FechaEvaluacion}, {examenActualizado.TipoExamen})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un examen con el ID {id} para actualizar.");
            }

            return Ok("Examen actualizado.");
        }
        [HttpDelete]
        [Route("EliminarExamen/{id}")]
        public async Task<IActionResult> EliminarExamen(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarExamen({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un examen con el ID {id} para eliminar.");
            }

            return Ok("Examen eliminado.");
        }

    }
}
