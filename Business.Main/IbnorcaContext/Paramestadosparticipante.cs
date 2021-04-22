using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Paramestadosparticipante
    {
        public short IdparamEstadoParticipante { get; set; }
        public string EstadoParticipante { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
