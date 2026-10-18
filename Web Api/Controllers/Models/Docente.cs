using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web_Api.Controllers.Models
{
    public class Docente
    {
        [Key]
        public int DocenteId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Apellidos { get; set; } = string.Empty;

        // Un docente puede tener muchas asignaciones
        [JsonIgnore]
        public ICollection<Asignacion>? Asignaciones { get; set; }
    }
}
