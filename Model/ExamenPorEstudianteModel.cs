using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class ExamenPorEstudianteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDExamen     {get; set;}
        public int IDEstudiante {get; set;}
        public float Nota { get; set; }
    }
}
