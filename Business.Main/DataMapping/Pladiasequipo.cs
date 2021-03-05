using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Pladiasequipo
    {
        public long IdPlAdiasEquipo { get; set; }
        public long? IdCicloParticipante { get; set; }
        public int? DiasInSitu { get; set; }
        public int? DiasRemoto { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
