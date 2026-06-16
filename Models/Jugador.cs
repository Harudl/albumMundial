using System.ComponentModel.DataAnnotations;

namespace albumMundial.Models
{
    public class Jugador
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del jugador es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La posición es obligatoria.")]
        public string Posicion { get; set; }

        [Range(1, 99, ErrorMessage = "El número de camiseta debe estar entre 1 y 99.")]
        public int NumeroCamiseta { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Foto del Jugador")]
        public string? FotoUrl { get; set; }

        // Relación: Un Jugador pertenece a un Equipo
        [Required]
        public int EquipoId { get; set; }
        public Equipo? Equipo { get; set; }

        // Relación: Un Jugador está en un Cromo
        public Cromo? Cromo { get; set; }
    }
}