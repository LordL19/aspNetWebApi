using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretariaController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;
        public SecretariaController (AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodasLasSecretarias")]
        public async Task<IActionResult> GetTodasLasSecretarias()
        {
            var lista = await appCnxinBD.Secretaria
                .FromSqlInterpolated($"CALL LeerSecretarias()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetSecretariaPorID/{id}")]
        public async Task<IActionResult> GetSecretariaPorID(int id)
        {
            var secretaria = await appCnxinBD.Secretaria
                .FromSqlRaw("SELECT * FROM secretaria WHERE IDSecretaria = {0}", id)
                .FirstOrDefaultAsync();

            if (secretaria == null)
            {
                return NotFound($"No se encontró una secretaria con el ID {id}");
            }

            return Ok(secretaria);
        }
        [HttpPost]
        [Route("CrearSecretaria")]
        public async Task<IActionResult> CrearSecretaria([FromBody] SecretariaModel nuevaSecretaria)
        {
            if (nuevaSecretaria == null)
            {
                return BadRequest("Los datos de la secretaria son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearSecretaria({nuevaSecretaria.Nombre}, {nuevaSecretaria.Apellido}, {nuevaSecretaria.IDNivel})");

            return Ok("Secretaria creada exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarSecretaria/{id}")]
        public async Task<IActionResult> ActualizarSecretaria(int id, [FromBody] SecretariaModel secretariaActualizada)
        {
            if (secretariaActualizada == null)
            {
                return BadRequest("Los datos de la secretaria son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarSecretaria({id}, {secretariaActualizada.Nombre}, {secretariaActualizada.Apellido}, {secretariaActualizada.IDNivel})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una secretaria con el ID {id} para actualizar.");
            }

            return Ok("Secretaria actualizada.");
        }
        [HttpDelete]
        [Route("EliminarSecretaria/{id}")]
        public async Task<IActionResult> EliminarSecretaria(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarSecretaria({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una secretaria con el ID {id} para eliminar.");
            }

            return Ok("Secretaria eliminada.");
        }

    }
}
