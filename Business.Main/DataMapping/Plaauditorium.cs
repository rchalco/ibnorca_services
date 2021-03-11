using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Plaauditorium
    {
        public long IdPlAauditoria { get; set; }
        public long? IidPrAcicloProgAuditoria { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDeRegistro { get; set; }
    }
}
