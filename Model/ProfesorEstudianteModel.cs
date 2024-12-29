using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspNetWebApi.Model
{
    public class ProfesorEstudianteModel
    {
       [Key]  
       public int IDProfesor            {get; set;}
       public int IDEstudiante          {get; set;}
       public int IDAsignaturaAsignada { get; set; }
    }
}
