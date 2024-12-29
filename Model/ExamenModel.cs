using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class ExamenModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDExamen         {get; set;}
        public int IDAsignatura     {get; set;}
        public int IDProfesor       {get; set;}
        public DateTime FechaEvaluacion  {get; set;}
        public string? TipoExamen { get; set; }
    }
}
