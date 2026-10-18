using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web_Api.Controllers.Models
{
    public class Estudiante
    {
        [Key]
        public int EstudianteId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Direccion { get; set; } = string.Empty;

        // Un estudiante puede tener muchas asignaciones
        [JsonIgnore]
        public ICollection<Asignacion>? Asignaciones { get; set; }
    }
}
