using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web_Api.Controllers.Models
{
    public class Curso
    {
        [Key]
        public int CursoId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        // Un curso puede tener muchas asignaciones
        [JsonIgnore]
        public ICollection<Asignacion>? Asignaciones { get; set; }
    }
}
