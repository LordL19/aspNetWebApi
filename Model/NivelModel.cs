using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class NivelModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDNivel          {get; set;}
        public string? Nombre           {get; set;}
        public int IDCoordinador    {get; set;}
        public int IDModulo         { get; set; }
        public int IDSecretaria { get; set; }
    }
}
