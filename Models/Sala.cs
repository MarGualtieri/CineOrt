using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva_Cine.Models
{
    public class Sala
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Número de sala")]
        public int Numero { get; set; }

        [ForeignKey(nameof(TipoSala))]
        [Display(Name = "Tipo de sala")]
        public Guid TipoSalaID { get; set; }
        [Display(Name = "Tipo de sala")]
        public TipoSala TipoSala { get; set; }

        [Display(Name = "Capacidad butacas")]
        public int CapacidadButacas { get; set; }

        public List<Funcion> Funciones { get; set; }
    }
}
