using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class ColegioModel
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       [JsonIgnore]
       public int IDColegio {get; set;}
       public string? Nombre { get; set; }
    }
}
