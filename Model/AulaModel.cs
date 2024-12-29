
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class AulaModel
    {
        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDAula       {get; set;}
        public string? Nombre       {get; set;}
        public int IDModulo     {get; set;}
        public int IDProfesor   { get; set; }
    }
}
