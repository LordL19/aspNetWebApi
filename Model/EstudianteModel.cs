using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class EstudianteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDEstudiante     {get; set;}
        public string? Nombre           {get; set;}
        public string? Apellido         {get; set;}
        public string? Grado            {get; set;}
        public string? NivelDeIngles    {get; set;}
        public int IDNivel { get; set; }
    }
}
