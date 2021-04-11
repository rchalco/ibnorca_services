using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elaauditorium
    {
        public Elaauditorium()
        {
            Elacontenidoauditoria = new HashSet<Elacontenidoauditorium>();
            Elacronogamas = new HashSet<Elacronogama>();
        }

        public int IdelaAuditoria { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
        public virtual Elaadp Elaadp { get; set; }
        public virtual Elahallazgo Elahallazgo { get; set; }
        public virtual ICollection<Elacontenidoauditorium> Elacontenidoauditoria { get; set; }
        public virtual ICollection<Elacronogama> Elacronogamas { get; set; }
    }
}
