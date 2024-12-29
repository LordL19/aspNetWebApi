using aspNetWebApi.Conn;
using aspNetWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorEstudianteController : ControllerBase
    {
        private readonly AppConexionBD appCnxinBD;
        public ProfesorEstudianteController(AppConexionBD appConexionBD)
        {
            appCnxinBD = appConexionBD;
        }
        [HttpGet]
        [Route("GetTodosLosProfesoresEstudiantes")]
        public async Task<IActionResult> GetTodosLosProfesoresEstudiantes()
        {
            var lista = await appCnxinBD.ProfesorEstudiante
                .FromSqlInterpolated($"CALL LeerProfesoresEstudiantes()")
                .ToListAsync();

            return Ok(lista);
        }
        [HttpGet]
        [Route("GetProfesorEstudiante/{idProfesor}/{idEstudiante}")]
        public async Task<IActionResult> GetProfesorEstudiante(int idProfesor, int idEstudiante)
        {
            var profesorEstudiante = await appCnxinBD.ProfesorEstudiante
                .FromSqlRaw("SELECT * FROM profesorestudiante WHERE IDProfesor = {0} AND IDEstudiante = {1}", idProfesor, idEstudiante)
                .FirstOrDefaultAsync();

            if (profesorEstudiante == null)
            {
                return NotFound($"No se encontró una relación profesor-estudiante con IDProfesor {idProfesor} e IDEstudiante {idEstudiante}");
            }

            return Ok(profesorEstudiante);
        }
        [HttpPost]
        [Route("CrearProfesorEstudiante")]
        public async Task<IActionResult> CrearProfesorEstudiante([FromBody] ProfesorEstudianteModel nuevaRelacion)
        {
            if (nuevaRelacion == null)
            {
                return BadRequest("Los datos de la relación son inválidos.");
            }

            await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL CrearProfesorEstudiante({nuevaRelacion.IDProfesor}, {nuevaRelacion.IDEstudiante}, {nuevaRelacion.IDAsignaturaAsignada})");

            return Ok("Relación profesor-estudiante creada exitosamente.");
        }
        [HttpPut]
        [Route("ActualizarProfesorEstudiante/{idProfesor}/{idEstudiante}")]
        public async Task<IActionResult> ActualizarProfesorEstudiante(int idProfesor, int idEstudiante, [FromBody] ProfesorEstudianteModel relacionActualizada)
        {
            if (relacionActualizada == null)
            {
                return BadRequest("Los datos de la relación son inválidos.");
            }

            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL ActualizarProfesorEstudiante({idProfesor}, {idEstudiante}, {relacionActualizada.IDAsignaturaAsignada})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una relación profesor-estudiante con IDProfesor {idProfesor} e IDEstudiante {idEstudiante} para actualizar.");
            }

            return Ok("Relación profesor-estudiante actualizada.");
        }
        [HttpDelete]
        [Route("EliminarProfesorEstudiante/{idProfesor}/{idEstudiante}")]
        public async Task<IActionResult> EliminarProfesorEstudiante(int idProfesor, int idEstudiante)
        {
            var resultado = await appCnxinBD.Database
                .ExecuteSqlInterpolatedAsync($"CALL EliminarProfesorEstudiante({idProfesor}, {idEstudiante})");

            if (resultado == 0)
            {
                return NotFound($"No se encontró una relación profesor-estudiante con IDProfesor {idProfesor} e IDEstudiante {idEstudiante} para eliminar.");
            }

            return Ok("Relación profesor-estudiante eliminada.");
        }
    }
}
