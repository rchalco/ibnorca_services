using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Paramestadosparticipante
    {
        public short IdparamEstadoParticipante { get; set; }
        public string EstadoParticipante { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
