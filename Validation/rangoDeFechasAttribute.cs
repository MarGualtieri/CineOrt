using System;
using System.ComponentModel.DataAnnotations;

namespace BENT1A.Grupo2.Validation
{
    public class rangoDeFechasAttribute : ValidationAttribute
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public rangoDeFechasAttribute()
        {
            _startDate = DateTime.Now; 
            _endDate = DateTime.Now.AddDays(7);
            ErrorMessage = "La fecha debe estar entre {0} y {1}.";
        }

        public override bool IsValid(object fecha)
        {
            Console.WriteLine("fecha");
            if (fecha is DateTime fechaReserva)
            {
                return fechaReserva > _startDate && fechaReserva <= _endDate;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, _endDate);
        }
    }
}