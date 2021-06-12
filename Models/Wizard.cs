using BENT1A.Grupo2.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva_Cine.Models
{
    public class Wizard
    {

        [ForeignKey(nameof(Funcion))]
        [Display(Name = "Función")]
        public Guid FuncionId { get; set; }
        public Funcion Funcion { get; set; }

        [ForeignKey(nameof(Pelicula))]
        [Display(Name = "Pelicula")]
        public Guid PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }

        [rangoDeFechas]
        [Required(ErrorMessage = "Este campo es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime Hora { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser de al menos {1}")]
        [Display(Name = "Cantidad de butacas")]
        public int CantidadButacas { get; set; }


    }
}
