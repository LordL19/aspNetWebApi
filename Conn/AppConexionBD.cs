using aspNetWebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace aspNetWebApi.Conn
{
    public class AppConexionBD : DbContext 
    {
        public AppConexionBD(DbContextOptions options) : base(options) { }

        public DbSet<AulaModel> Aula { get; set; }

        public DbSet<AsignaturaModel> Asignatura { get; set; }

        public DbSet<AsignaturaAsignadaModel> AsignaturaAsignada { get; set; }

        public DbSet<ColegioModel> Colegio { get; set; }

        public DbSet<CoordinadorModel> Coordinador { get; set; }

        public DbSet<DirectorGeneralModel> DirectorGeneral { get; set; }

        public DbSet<EstudianteModel> Estudiante { get; set; }

        public DbSet<ExamenModel> Examen { get; set; }

        public DbSet<ExamenPorEstudianteModel> ExamenPorEstudiante { get; set; }

        public DbSet<HorarioAulaPorAsignaturaModel> HorarioAulaPorAsignatura { get; set; }

        public DbSet<ModuloModel> Modulo { get; set; }

        public DbSet<NivelModel> Nivel { get; set; }

        public DbSet<ProfesorModel> Profesor { get; set; }

        public DbSet<ProfesorEstudianteModel> ProfesorEstudiante { get; set; }

        public DbSet<SecretariaModel> Secretaria { get; set; }

    }
}
