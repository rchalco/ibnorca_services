using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Pcargosparticipante
    {
        public short IdpCargoParticipante { get; set; }
        public string CargoParticipante { get; set; }
        public short? IdpTipoCertificacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
