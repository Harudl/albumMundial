using System.ComponentModel.DataAnnotations;

namespace albumMundial.Models
{
    public class Equipo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El director técnico es obligatorio.")]
        public string DirectorTecnico { get; set; }

        public int AnioFundacion { get; set; }

        public string? LogoUrl { get; set; } // Guardaremos la ruta de la imagen (ej: /images/logos/ecuador.png)

        [Required(ErrorMessage = "El grupo mundialista es obligatorio.")]
        [RegularExpression(@"^[A-H]$", ErrorMessage = "El grupo debe ser una letra entre la A y la H.")]
        public string GrupoMundialista { get; set; }

        // Relación: Un Equipo pertenece a un País
        [Required]
        public int PaisId { get; set; }
        public Pais? Pais { get; set; }

        // Relación: Un Equipo tiene muchos Jugadores
        public List<Jugador> Jugadores { get; set; } = new List<Jugador>();
    }
}
