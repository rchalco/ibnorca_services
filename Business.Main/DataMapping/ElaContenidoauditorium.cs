using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elacontenidoauditorium
    {
        public int IdelaContenidoauditoria { get; set; }
        public int? IdelaAuditoria { get; set; }
        public string Nemotico { get; set; }
        public string Contenido { get; set; }
        public ulong? Seleccionado { get; set; }

        public virtual Elaauditorium IdelaAuditoriaNavigation { get; set; }
    }
}
