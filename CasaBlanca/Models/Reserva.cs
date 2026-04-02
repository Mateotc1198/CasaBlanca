using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasaBlanca.Models
{
    public class Reserva : IValidatableObject
    {
        public int IdReserva { get; set; }
        public int? IdUsuario { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un hotel.")]
        public int IdHotel { get; set; }
        public string Destino { get; set; }

        [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime? Entrada { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime? Salida { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la cantidad de huéspedes.")]
        public string Huespedes { get; set; }

        public string Estado { get; set; }

        public string NombreUsuario { get; set; }
        public string CorreoUsuario { get; set; }
        public string NombreHotel { get; set; }
        public string ImagenHotel { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public int Noches { get; set; }
        public decimal Total { get; set; }
 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Entrada.HasValue && Salida.HasValue)
            {
                if (Salida.Value < Entrada.Value)
                {
                    yield return new ValidationResult(
                        "La fecha de salida no puede ser menor que la fecha de entrada.",
                        new[] { "Salida" }
                    );
                }

                if (Entrada.Value.Date < DateTime.Today)
                {
                    yield return new ValidationResult(
                        "La fecha de entrada no puede ser anterior a hoy.",
                        new[] { "Entrada" }
                    );
                }
            }
        }
    }
}