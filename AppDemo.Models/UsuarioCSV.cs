using System;

namespace AppDemo.Models
{
    public class UsuarioCSV
    {
        public int IdUsuario { get; set; }
        public int? HorasTrabajadas { get; set; }
        public decimal? SueldoPorHora { get; set; }
        public DateTime? FechaProximoPago { get; set; }
    }
}
