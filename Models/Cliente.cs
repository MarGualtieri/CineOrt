using System;
using System.Collections.Generic;

namespace Reserva_Cine.Models
{

    public class Cliente : Usuario
    {
        public List<Reserva> Reservas { get; set; }
        public Guid? Id { get; internal set; }
    }
}
