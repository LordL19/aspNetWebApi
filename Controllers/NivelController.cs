using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NivelController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;
        public NivelController (AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosNiveles")]
        public async Task<IActionResult> GetTodosLosNiveles()
        {
            var lista = await appCnxinBD.Nivel
                .FromSqlInterpolated($"CALL LeerNiveles()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetNivelPorID/{id}")]
        public async Task<IActionResult> GetNivelPorID(int id)
        {
            var nivel = await appCnxinBD.Nivel
                .FromSqlRaw("SELECT * FROM nivel WHERE IDNivel = {0}",id)
                .FirstOrDefaultAsync();

            if (nivel == null)
            {
                return NotFound($"No se encontró un nivel con el ID {id}");
            }

            return Ok(nivel);
        }
        [HttpPost]
        [Route("CrearNivel")]
        public async Task<IActionResult> CrearNivel([FromBody] NivelModel nuevoNivel)
        {
            if (nuevoNivel == null)
            {
                return BadRequest("Los datos del nivel son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearNivel({nuevoNivel.Nombre}, {nuevoNivel.IDCoordinador}, {nuevoNivel.IDModulo}, {nuevoNivel.IDSecretaria})");

            return Ok("Nivel creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarNivel/{id}")]
        public async Task<IActionResult> ActualizarNivel(int id, [FromBody] NivelModel nivelActualizado)
        {
            if (nivelActualizado == null)
            {
                return BadRequest("Los datos del nivel son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarNivel({id}, {nivelActualizado.Nombre}, {nivelActualizado.IDCoordinador}, {nivelActualizado.IDModulo}, {nivelActualizado.IDSecretaria})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un nivel con el ID {id} para actualizar.");
            }

            return Ok("Nivel actualizado.");
        }
        [HttpDelete]
        [Route("EliminarNivel/{id}")]
        public async Task<IActionResult> EliminarNivel(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarNivel({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un nivel con el ID {id} para eliminar.");
            }

            return Ok("Nivel eliminado.");
        }

    }
}
