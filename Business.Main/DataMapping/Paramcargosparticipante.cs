using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Paramcargosparticipante
    {
        public short IdparamCargoParticipante { get; set; }
        public string CargoParticipante { get; set; }
        public short? IdpTipoCertificacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
