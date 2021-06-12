using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reserva_Cine.Models
{
    // TODO: Recordar que tal como se dijo en clase TODOS los campos de tipo string deben tener especificado un MaxLength
    public class Genero
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        public List<Pelicula> Peliculas { get; set; }
    }
}
