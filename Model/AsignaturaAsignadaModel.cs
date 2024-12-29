using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class AsignaturaAsignadaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDAsignaturaAsignada {get; set;}
        public int IDNivel              {get; set;}
        public int IDProfesor           {get; set;}
        public int IDAsignatura { get; set; }
    }
}
