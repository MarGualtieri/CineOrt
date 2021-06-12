using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserva_Cine.Models
{
    public class TipoSala
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "mensaje de error")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        // no está mal utilizar float, pero como comenté en clase, usaremos decimal para los montos de dinero.
        [Required(ErrorMessage = "mensaje de error")]
        public decimal Precio { get; set; }

        public List<Sala> Salas { get; set; }
    }
}
