using Reserva_Cine.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Reserva_Cine.Models
{
    public abstract class Usuario
    {
        [Key]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        // [RegularExpression(@"[123456789]*", ErrorMessage = "El campo admite sólo caracteres numéricos")]
        [MaxLength(8, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        [Display(Name = "DNI")]
        public string DNI { get; set; }



        // [RegularExpression(@"^(\+[0-9]{13})$", ErrorMessage = "El siguiente debe ser un número de telefono")]
        [MaxLength(14, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Telefono { get; set; }



        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo es de 100 caracteres")]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = "El campo admite sólo caracteres alfabéticos")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }



        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo es de 100 caracteres")]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = "El campo admite sólo caracteres alfabéticos")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Display(Name = "Password")]
        [ScaffoldColumn(false)] // se agrega este atributo para que no se considere en los scaffolding esta propiedad
        public byte[] Password { get; set; } // La password la usaremos como un array de bytes ya que se almacenara en la base de datos encriptada e ilegible

        [Required(ErrorMessage = "Este campo es requerido")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un email válido")] // utilizamos esta validación para los emails
        [MaxLength(320, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Alta")]
        [DisplayFormat(DataFormatString ="{0:dd/mm/yyyy H:mm}" )]
        public DateTime FechaAlta { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Fecha de ultima modificion")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy H:mm}")]
        public DateTime FechaUltimaModificacion { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Fecha de ultima conexion")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy H:mm}")]
        public DateTime FechaUltimoLoggin { get; set; }

      

        [MaxLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Direccion { get; set; }


    }
}
