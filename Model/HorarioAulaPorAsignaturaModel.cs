using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class HorarioAulaPorAsignaturaModel
    {
           [Key]
           [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
           [JsonIgnore]
           public int IDHorario     {get; set;}
           public int IDAula        {get; set;}
           public int IDAsignatura  {get; set;}
           public TimeSpan HoraInicio    {get; set;}
           public TimeSpan HoraFin { get; set; }
    }
}
