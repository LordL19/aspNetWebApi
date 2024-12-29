using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenPorEstudianteController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public ExamenPorEstudianteController (AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosExamenesPorEstudiantes")]
        public async Task<IActionResult> GetTodosLosExamenesPorEstudiantes()
        {
            var lista = await appCnxinBD.ExamenPorEstudiante
                .FromSqlInterpolated($"CALL LeerExamenesPorEstudiantes()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetExamenPorEstudiante/{idExamen}/{idEstudiante}")]
        public async Task<IActionResult> GetExamenPorEstudiante(int idExamen, int idEstudiante)
        {
            var examenPorEstudiante = await appCnxinBD.ExamenPorEstudiante
                .FromSqlRaw("SELECT * FROM examenporestudiante WHERE IDExamen = {0} AND IDEstudiante = {1}", idExamen, idEstudiante)
                .FirstOrDefaultAsync();

            if (examenPorEstudiante == null)
            {
                return NotFound($"No se encontró un examen con IDExamen {idExamen} y IDEstudiante {idEstudiante}");
            }

            return Ok(examenPorEstudiante);
        }
        [HttpPost]
        [Route("CrearExamenPorEstudiante")]
        public async Task<IActionResult> CrearExamenPorEstudiante([FromBody] ExamenPorEstudianteModel nuevoExamenPorEstudiante)
        {
            if (nuevoExamenPorEstudiante == null)
            {
                return BadRequest("Los datos del examen por estudiante son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearExamenPorEstudiante({nuevoExamenPorEstudiante.IDExamen}, {nuevoExamenPorEstudiante.IDEstudiante}, {nuevoExamenPorEstudiante.Nota})");

            return Ok("Examen por estudiante creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarNotaExamenPorEstudiante/{idExamen}/{idEstudiante}")]
        public async Task<IActionResult> ActualizarNotaExamenPorEstudiante(int idExamen, int idEstudiante, [FromBody] ExamenPorEstudianteModel examenActualizado)
        {
            if (examenActualizado == null)
            {
                return BadRequest("Los datos del examen por estudiante son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarNotaExamenPorEstudiante({idExamen}, {idEstudiante}, {examenActualizado.Nota})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un examen con IDExamen {idExamen} y IDEstudiante {idEstudiante} para actualizar.");
            }

            return Ok("Nota de examen por estudiante actualizada.");
        }
        [HttpDelete]
        [Route("EliminarExamenPorEstudiante/{idExamen}/{idEstudiante}")]
        public async Task<IActionResult> EliminarExamenPorEstudiante(int idExamen, int idEstudiante)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarExamenPorEstudiante({idExamen}, {idEstudiante})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un examen con IDExamen {idExamen} y IDEstudiante {idEstudiante} para eliminar.");
            }

            return Ok("Examen por estudiante eliminado.");
        }

    }
}
