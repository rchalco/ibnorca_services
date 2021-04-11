using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elahallazgo
    {
        public int Idelahallazgo { get; set; }
        public int? IdelaAuditoria { get; set; }
        public string Tipo { get; set; }
        public string Normas { get; set; }
        public string Proceso { get; set; }
        public string Hallazgo { get; set; }
        public string Sitio { get; set; }
        public DateTime? Fecha { get; set; }
        public string Usuario { get; set; }

        public virtual Elaauditorium IdelahallazgoNavigation { get; set; }
    }
}
