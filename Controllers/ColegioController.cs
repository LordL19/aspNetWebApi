using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColegioController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public ColegioController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosColegios")]
        public async Task<IActionResult> GetTodosLosColegios()
        {
            var list_de_colegios = await appCnxinBD.Colegio
                .FromSqlInterpolated($"CALL LeerColegios()")
                .ToListAsync();

            return Ok(list_de_colegios);    
        }
        [HttpGet]
        [Route("GetColegioPorID/{id}")]
        public async Task<IActionResult> GetColegioPorID(int id)
        {
            var colegio = await appCnxinBD.Colegio
                .FromSqlRaw("SELECT * FROM colegio WHERE IDColegio = {0}", id)
                .FirstOrDefaultAsync();

            if(colegio == null)
            {
                return NotFound($"No se encontro un colegio con el ID {id}");
            }
            return Ok(colegio);
        }
        [HttpPost]
        [Route("CrearColegio")]
        public async Task<IActionResult> CrearColegio([FromBody] ColegioModel nuevoColegio)
        {
            if (nuevoColegio == null)
            {
                return BadRequest("Los datos del colegio son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearColegio({nuevoColegio.Nombre})");

            return Ok("Colegio creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarColegio/{id}")]
        public async Task<IActionResult> ActualizarColegio(int id, [FromBody] ColegioModel colegioActualizado)
        {
            if (colegioActualizado == null)
            {
                return BadRequest("Los datos del colegio son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarColegio({id}, {colegioActualizado.Nombre})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un colegio con el ID {id} para actualizar.");
            }

            return Ok("Colegio actualizado.");
        }
        [HttpDelete]
        [Route("EliminarColegio/{id}")]
        public async Task<IActionResult> EliminarColegio(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarColegio({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un colegio con el ID {id} para eliminar.");
            }

            return Ok("Colegio eliminado.");
        }



    }
}
