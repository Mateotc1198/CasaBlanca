using System.ComponentModel.DataAnnotations;

namespace CasaBlanca.Models
{
    public class Hotel
    {
        public int IdHotel { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string Ciudad { get; set; }

        [Range(1, 999999, ErrorMessage = "El precio por noche debe ser mayor que cero.")]
        public decimal PrecioPorNoche { get; set; }

        [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20.")]
        public int CapacidadMaxima { get; set; }

        public string Imagen { get; set; }
        public bool Activo { get; set; }
    }
}