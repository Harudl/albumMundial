using System.ComponentModel.DataAnnotations;

namespace albumMundial.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del álbum es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        [Display(Name = "Nombre del Álbum")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        [Range(1930, 2030, ErrorMessage = "Por favor, introduce un año mundialista válido.")]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "La cantidad total de cromos es obligatoria.")]
        [Range(1, 1000, ErrorMessage = "La cantidad de cromos debe ser un número entre 1 y 1000.")]
        [Display(Name = "Cantidad de Cromos")]
        public int CantidadCromos { get; set; }

        [Display(Name = "Edición Especial")]
        public bool EdicionEspecial { get; set; } // True si es edición oro, platino, etc.

        // RELACIONES
        // Relación: Un Álbum contiene muchos Cromos
        public List<Cromo> Cromos { get; set; } = new List<Cromo>();
    }
}
