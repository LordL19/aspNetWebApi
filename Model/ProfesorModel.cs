using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class ProfesorModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDProfesor           {get; set;}
        public string? Nombre               {get; set;}
        public string? Apellido             {get; set;}
        public string? Especialidad         {get; set;}
        public int IDAula               {get; set;}
        public int IDNivel              {get; set;}
        public int IDAsignaturaAsignada { get; set; }
    }
}
