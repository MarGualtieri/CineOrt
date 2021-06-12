using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva_Cine.Models
{
    public class Funcion
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }


        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime Hora { get; set; }


        // 200 sería un length valido en este campo?
        [MaxLength(200, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Descripcion { get; set; }

        [Display(Name = "Butacas disponibles")]
        public int ButacasDisponibles { get; set; }

        public bool Confirmada { get; set; }

        [ForeignKey(nameof(Pelicula))]
        [Display(Name = "Pelicula")]
        public Guid PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }


        [ForeignKey(nameof(Sala))]
        [Display(Name = "Sala")]
        public Guid SalaId { get; set; }
        public Sala Sala { get; set; }

        public List<Reserva> Reservas { get; set; }
    }

}
