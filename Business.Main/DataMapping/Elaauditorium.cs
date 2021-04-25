using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elaauditorium
    {
        public Elaauditorium()
        {
            Elaadps = new HashSet<Elaadp>();
            Elacontenidoauditoria = new HashSet<Elacontenidoauditorium>();
            Elacronogamas = new HashSet<Elacronogama>();
            Elahallazgos = new HashSet<Elahallazgo>();
            Tmddocumentacionauditoria = new HashSet<Tmddocumentacionauditorium>();
        }

        public int IdelaAuditoria { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
        public virtual ICollection<Elaadp> Elaadps { get; set; }
        public virtual ICollection<Elacontenidoauditorium> Elacontenidoauditoria { get; set; }
        public virtual ICollection<Elacronogama> Elacronogamas { get; set; }
        public virtual ICollection<Elahallazgo> Elahallazgos { get; set; }
        public virtual ICollection<Tmddocumentacionauditorium> Tmddocumentacionauditoria { get; set; }
    }
}
