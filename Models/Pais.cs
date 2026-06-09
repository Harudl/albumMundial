using System.ComponentModel.DataAnnotations;

namespace albumMundial.Models
{
    public class Pais
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del país es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El continente es obligatorio.")]
        public string Continente { get; set; }

        [Required(ErrorMessage = "El código FIFA es obligatorio.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El código FIFA debe tener exactamente 3 letras.")]
        public string CodigoFifa { get; set; }

        [Range(1, 300, ErrorMessage = "El ranking debe ser un número válido.")]
        public int RankingFifa { get; set; }

        // Relación: Un País puede tener un Equipo en el mundial
        public Equipo? Equipo { get; set; }
    }
}