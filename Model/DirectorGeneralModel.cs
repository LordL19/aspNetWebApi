using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace aspNetWebApi.Model
{
    public class DirectorGeneralModel
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       [JsonIgnore]
       public int  IDDirector   {get; set;}
       public string?  Nombre       {get; set;}
       public string?  Apellido     {get; set;}
       public int  IDSecretaria { get; set; }
    }
}
