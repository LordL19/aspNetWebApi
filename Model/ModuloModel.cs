using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class ModuloModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDModulo         {get; set;}
        public string? Nombre           {get; set;}
        public int CantidadAulas    {get; set;}
        public int IDNivel { get; set; }
    }
}
