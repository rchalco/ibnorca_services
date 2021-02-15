using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Ptipoauditorium
    {
        public short IdpTipoAuditoria { get; set; }
        public string TipoAuditoria { get; set; }
        public int? IdpTipoCertificacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
