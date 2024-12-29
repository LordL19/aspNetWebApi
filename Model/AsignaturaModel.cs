using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class AsignaturaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int IDAsignatura { get; set; }
        public string? Nombre { get; set; }
        public int IDAula { get; set; }
    }
}
