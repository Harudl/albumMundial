using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace albumMundial.Models
{
    public class Cromo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de cromo es obligatorio.")]
        [Display(Name = "Número de Cromo")]
        public string NumeroCromo { get; set; } // Puede ser "ECU 1", "ARG 10", o solo números

        [Required(ErrorMessage = "La edición del cromo es obligatoria.")]
        public string Edicion { get; set; } // Ej: Estándar, Brillante, Legendario

        [Required(ErrorMessage = "El valor de mercado es obligatorio.")]
        [Range(0.01, 10000.00, ErrorMessage = "El valor debe ser un número positivo válido.")]
        [Column(TypeName = "decimal(18,2)")] // Configura el tipo de dato correcto en SQL Server
        [Display(Name = "Valor de Mercado ($)")]
        public decimal ValorMercado { get; set; }

        [Display(Name = "Foto del Cromo")]
        public string? FotoUrl { get; set; } 

        // RELACIONES Y LLAVES FORÁNEAS

        // Relación 1-a-1 o 1-a-Muchos: Un Cromo pertenece a un Jugador
        [Required(ErrorMessage = "Debe asociar este cromo a un jugador.")]
        [Display(Name = "Jugador")]
        public int JugadorId { get; set; }
        public Jugador? Jugador { get; set; }

        // Relación: Un Cromo pertenece a un Álbum
        [Required(ErrorMessage = "Debe asignar el cromo a un álbum.")]
        [Display(Name = "Álbum")]
        public int AlbumId { get; set; }
        public Album? Album { get; set; }
    }
}
