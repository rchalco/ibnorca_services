using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elaadp
    {
        public int Idelaadp { get; set; }
        public int? IdelaAuditoria { get; set; }
        public string Area { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public string Usuario { get; set; }

        public virtual Elaauditorium IdelaAuditoriaNavigation { get; set; }
    }
}
