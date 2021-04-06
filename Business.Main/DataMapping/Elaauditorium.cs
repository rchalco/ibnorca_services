using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elaauditorium
    {
        public Elaauditorium()
        {
            ElaContenidoauditoria = new HashSet<ElaContenidoauditorium>();
            Elacronogamas = new HashSet<Elacronogama>();
        }

        public int IdelaAuditoria { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
        public virtual ICollection<ElaContenidoauditorium> ElaContenidoauditoria { get; set; }
        public virtual ICollection<Elacronogama> Elacronogamas { get; set; }
    }
}
