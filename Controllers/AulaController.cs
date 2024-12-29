using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AulaController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public AulaController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodasLasAulasEF")]

        public async Task<IActionResult> GetTodasLasAulasEF()
        {
            var lista_de_aulas = await appCnxinBD.Aula.ToListAsync();

            return Ok(lista_de_aulas);
        }
        [HttpGet]
        [Route("GetTodasLasAulasSP")]
        public async Task<IActionResult> GetTodasLasAulasSP()
        {
            var lista_de_aulas = await appCnxinBD.Aula.FromSqlInterpolated($"CALL LeerAulas()").ToListAsync();

            return Ok(lista_de_aulas);
        }

        [HttpGet]
        [Route("GetAulaPorID/{id}")]
        public async Task<IActionResult> GetAulaPorID(int id)
        {
            try
            {
                var aula = await appCnxinBD.Aula
                .FromSqlRaw("SELECT * FROM aula WHERE IDAula = {0}", id)
                .FirstOrDefaultAsync();

                if (aula == null)
                {
                    return NotFound($"No se encontró un aula con el ID {id}");
                }

                return Ok(aula);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("CrearAula")]
        public async Task<IActionResult> CrearAula([FromBody] AulaModel nuevaAula)
        {
            if (nuevaAula == null)
            {
                return BadRequest("Los datos del aula son invalidos");
            }

            await appCnxinBD.Database.
                ExecuteSqlInterpolatedAsync($"CALL CrearAula({nuevaAula.Nombre}, {nuevaAula.IDModulo}, {nuevaAula.IDProfesor})");
            return Ok("Aula Creada exitosamente");
        }

        [HttpPut]
        [Route("ActualizarAula/{id}")]
        public async Task<IActionResult> ActualizarAula(int id, [FromBody] AulaModel aulaActualizada)
        {
            if (aulaActualizada == null)
            {
                return BadRequest("Los datos del aula son invalidos");
            }
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarAula({id}, {aulaActualizada.Nombre}, {aulaActualizada.IDModulo}, {aulaActualizada.IDProfesor})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un aula con el ID {id} para actualizar.");
            }

            return Ok("Aula actualizada.");
        }
        [HttpDelete]
        [Route("EliminarAula/{id}")]
        public async Task<IActionResult> EliminarAula(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarAula({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un aula con el ID {id} para eliminar.");
            }

            return Ok("Aula eliminada.");
        }
    }
}
