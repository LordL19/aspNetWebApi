using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuloController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public ModuloController (AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }

        [HttpGet]
        [Route("GetTodosLosModulos")]
        public async Task<IActionResult> GetTodosLosModulos()
        {
            var lista = await appCnxinBD.Modulo
                .FromSqlInterpolated($"CALL LeerModulos()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetModuloPorID/{id}")]
        public async Task<IActionResult> GetModuloPorID(int id)
        {
            var modulo = await appCnxinBD.Modulo
                .FromSqlRaw("SELECT * FROM modulo WHERE IDModulo = {0}", id)
                .FirstOrDefaultAsync();

            if (modulo == null)
            {
                return NotFound($"No se encontró un módulo con el ID {id}");
            }

            return Ok(modulo);
        }
        [HttpPost]
        [Route("CrearModulo")]
        public async Task<IActionResult> CrearModulo([FromBody] ModuloModel nuevoModulo)
        {
            if (nuevoModulo == null)
            {
                return BadRequest("Los datos del módulo son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearModulo({nuevoModulo.Nombre}, {nuevoModulo.CantidadAulas}, {nuevoModulo.IDNivel})");

            return Ok("Módulo creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarModulo/{id}")]
        public async Task<IActionResult> ActualizarModulo(int id, [FromBody] ModuloModel moduloActualizado)
        {
            if (moduloActualizado == null)
            {
                return BadRequest("Los datos del módulo son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarModulo({id}, {moduloActualizado.Nombre}, {moduloActualizado.CantidadAulas}, {moduloActualizado.IDNivel})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un módulo con el ID {id} para actualizar.");
            }

            return Ok("Módulo actualizado.");
        }
        [HttpDelete]
        [Route("EliminarModulo/{id}")]
        public async Task<IActionResult> EliminarModulo(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarModulo({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un módulo con el ID {id} para eliminar.");
            }

            return Ok("Módulo eliminado.");
        }
    }
}
