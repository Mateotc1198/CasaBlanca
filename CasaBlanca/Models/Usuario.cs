using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CasaBlanca.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Clave { get; set; }

        public bool EsAdmin { get; set; }
    }
}