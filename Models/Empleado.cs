using System;
using System.ComponentModel.DataAnnotations;

namespace Reserva_Cine.Models
{
    // El código debe compilar para que el proyecto ejecute, de lo contrario no ejecutará la aplicación.
    // En este caso el código no compilaba ya que nos faltaba la directiva using System.ComponentModel.DataAnnotations;
    // TODO: Recordar que tal como se dijo en clase TODOS los campos de tipo string deben tener especificado un MaxLength
    public class Empleado : Usuario
    {
        [Required(ErrorMessage = "mensaje de error")]
        public Guid Legajo { get; set; }
    }
}
