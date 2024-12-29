using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorGeneralController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public DirectorGeneralController (AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosDirectoresGenerales")]
        public async Task<IActionResult> GetTodosLosDirectoresGenerales()
        {
            var lista = await appCnxinBD.DirectorGeneral
                .FromSqlInterpolated($"CALL LeerDirectoresGenerales()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetDirectorGeneralPorID/{id}")]
        public async Task<IActionResult> GetDirectorGeneralPorID(int id)
        {
            var directorGeneral = await appCnxinBD.DirectorGeneral
                .FromSqlRaw("SELECT * FROM directorgeneral WHERE IDDirector = {0}", id)
                .FirstOrDefaultAsync();

            if (directorGeneral == null)
            {
                return NotFound($"No se encontró un director general con el ID {id}");
            }

            return Ok(directorGeneral);
        }
        [HttpPost]
        [Route("CrearDirectorGeneral")]
        public async Task<IActionResult> CrearDirectorGeneral([FromBody] DirectorGeneralModel nuevoDirectorGeneral)
        {
            if (nuevoDirectorGeneral == null)
            {
                return BadRequest("Los datos del director general son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearDirectorGeneral({nuevoDirectorGeneral.Nombre}, {nuevoDirectorGeneral.Apellido}, {nuevoDirectorGeneral.IDSecretaria})");

            return Ok("Director general creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarDirectorGeneral/{id}")]
        public async Task<IActionResult> ActualizarDirectorGeneral(int id, [FromBody] DirectorGeneralModel directorGeneralActualizado)
        {
            if (directorGeneralActualizado == null)
            {
                return BadRequest("Los datos del director general son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarDirectorGeneral({id}, {directorGeneralActualizado.Nombre}, {directorGeneralActualizado.Apellido}, {directorGeneralActualizado.IDSecretaria})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un director general con el ID {id} para actualizar.");
            }

            return Ok("Director general actualizado.");
        }
        [HttpDelete]
        [Route("EliminarDirectorGeneral/{id}")]
        public async Task<IActionResult> EliminarDirectorGeneral(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarDirectorGeneral({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un director general con el ID {id} para eliminar.");
            }

            return Ok("Director general eliminado.");
        }


    }
}
