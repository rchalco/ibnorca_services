using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Placronoequipo
    {
        public long IdPlAcronoEquipo { get; set; }
        public long? IdPlAcronograma { get; set; }
        public long? IdCicloParticipante { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
