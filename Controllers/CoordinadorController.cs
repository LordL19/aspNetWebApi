using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoordinadorController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public CoordinadorController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }

        [HttpGet]
        [Route("GetTodosLosCoordinadores")]
        public async Task<IActionResult> GetTodosLosCoordinadores()
        {
            var lista_de_coordinadores = await appCnxinBD.Coordinador
                .FromSqlInterpolated($"CALL LeerCoordinadores()")
                .ToListAsync();

            return Ok(lista_de_coordinadores);
        }
        [HttpGet]
        [Route("GetCoordinadorPorID/{id}")]
        public async Task<IActionResult> GetCoordinadorPorID(int id)
        {
            var coordinador = await appCnxinBD.Coordinador
                .FromSqlRaw("SELECT * FROM coordinador WHERE IDCoordinador = {0}", id)
                .FirstOrDefaultAsync();

            if (coordinador == null)
            {
                return NotFound($"No se encontró un coordinador con el ID {id}");
            }

            return Ok(coordinador);
        }
        [HttpPost]
        [Route("CrearCoordinador")]
        public async Task<IActionResult> CrearCoordinador([FromBody] CoordinadorModel nuevoCoordinador)
        {
            if (nuevoCoordinador == null)
            {
                return BadRequest("Los datos del coordinador son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearCoordinador({nuevoCoordinador.Nombre}, {nuevoCoordinador.Apellido}, {nuevoCoordinador.IDNivel}, {nuevoCoordinador.IDSecretaria})");

            return Ok("Coordinador creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarCoordinador/{id}")]
        public async Task<IActionResult> ActualizarCoordinador(int id, [FromBody] CoordinadorModel coordinadorActualizado)
        {
            if (coordinadorActualizado == null)
            {
                return BadRequest("Los datos del coordinador son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarCoordinador({id}, {coordinadorActualizado.Nombre}, {coordinadorActualizado.Apellido}, {coordinadorActualizado.IDNivel}, {coordinadorActualizado.IDSecretaria})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un coordinador con el ID {id} para actualizar.");
            }

            return Ok("Coordinador actualizado.");
        }
        [HttpDelete]
        [Route("EliminarCoordinador/{id}")]
        public async Task<IActionResult> EliminarCoordinador(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarCoordinador({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un coordinador con el ID {id} para eliminar.");
            }

            return Ok("Coordinador eliminado.");
        }


    }
}
