using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioAulaPorAsignaturaController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;

        public HorarioAulaPorAsignaturaController (AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosHorariosAulaPorAsignatura")]
        public async Task<IActionResult> GetTodosLosHorariosAulaPorAsignatura()
        {
            var lista = await appCnxinBD.HorarioAulaPorAsignatura
                .FromSqlInterpolated($"CALL LeerHorariosAulaPorAsignatura()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetHorarioAulaPorAsignatura/{id}")]
        public async Task<IActionResult> GetHorarioAulaPorAsignatura(int id)
        {
            var horario = await appCnxinBD.HorarioAulaPorAsignatura
                .FromSqlRaw("SELECT * FROM horarioaulaporasignatura WHERE IDHorario = {0}", id)
                .FirstOrDefaultAsync();

            if (horario == null)
            {
                return NotFound($"No se encontró un horario con el ID {id}");
            }

            return Ok(horario);
        }
        [HttpPost]
        [Route("CrearHorarioAulaPorAsignatura")]
        public async Task<IActionResult> CrearHorarioAulaPorAsignatura([FromBody] HorarioAulaPorAsignaturaModel nuevoHorario)
        {
            if (nuevoHorario == null)
            {
                return BadRequest("Los datos del horario son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearHorarioAulaPorAsignatura({nuevoHorario.IDAula}, {nuevoHorario.IDAsignatura}, {nuevoHorario.HoraInicio}, {nuevoHorario.HoraFin})");

            return Ok("Horario aula por asignatura creado exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarHorarioAulaPorAsignatura/{id}")]
        public async Task<IActionResult> ActualizarHorarioAulaPorAsignatura(int id, [FromBody] HorarioAulaPorAsignaturaModel horarioActualizado)
        {
            if (horarioActualizado == null)
            {
                return BadRequest("Los datos del horario son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarHorarioAulaPorAsignatura({id}, {horarioActualizado.IDAula}, {horarioActualizado.IDAsignatura}, {horarioActualizado.HoraInicio}, {horarioActualizado.HoraFin})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un horario con el ID {id} para actualizar.");
            }

            return Ok("Horario aula por asignatura actualizado.");
        }
        [HttpDelete]
        [Route("EliminarHorarioAulaPorAsignatura/{id}")]
        public async Task<IActionResult> EliminarHorarioAulaPorAsignatura(int id)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarHorarioAulaPorAsignatura({id})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró un horario con el ID {id} para eliminar.");
            }

            return Ok("Horario aula por asignatura eliminado.");
        }
    }
}
