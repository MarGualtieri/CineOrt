using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BENT1A.Grupo2.Models.Enums;

namespace Reserva_Cine.Models
{
    public class Reserva
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Funcion))]
        [Display(Name = "Función")]
        public Guid FuncionId { get; set; }
        public Funcion Funcion { get; set; }

        [Display(Name = "Alta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime FechaAlta { get; set; }

        [ForeignKey(nameof(Cliente))]
        [Display(Name = "Cliente")]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Display(Name = "Cantidad de butacas")]
        public int CantidadButacas { get; set; }

        public EstadoReserva Estado {get; set;}
    }
}
