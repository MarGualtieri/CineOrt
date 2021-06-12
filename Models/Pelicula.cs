using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reserva_Cine.Models
{
    // TODO: Recordar que tal como se dijo en clase TODOS los campos de tipo string deben tener especificado un MaxLength
    public class Pelicula
    {
        [Key]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "mensaje de error")]
        [Display(Name = "Fecha de lanzamiento")]
        public DateTime FechaLanzamiento { get; set; }
        
        [Required(ErrorMessage = "mensaje de error")]
        [MaxLength(215, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Titulo { get; set; }

        [MaxLength(200, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Descripcion { get; set; }

        [ForeignKey(nameof(Genero))]
        [Display(Name = "Genero")]
        public Guid GeneroId { get; set; }
        public Genero Genero { get; set; }

        public List<Funcion> Funciones { get; set; }
    }
}
