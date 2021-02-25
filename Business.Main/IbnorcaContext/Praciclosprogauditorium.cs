using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Praciclosprogauditorium
    {
        public Praciclosprogauditorium()
        {
            Praciclocronogramas = new HashSet<Praciclocronograma>();
            Praciclonormassistemas = new HashSet<Praciclonormassistema>();
            Pracicloparticipantes = new HashSet<Pracicloparticipante>();
            Pradireccionespaproductos = new HashSet<Pradireccionespaproducto>();
            Pradireccionespasistemas = new HashSet<Pradireccionespasistema>();
        }

        public long IdPrAcicloProgAuditoria { get; set; }
        public long? IdPrAprogramaAuditoria { get; set; }
        public string NombreOrganizacionCertificado { get; set; }
        public short? Ano { get; set; }
        public short? IdparamTipoAuditoria { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual Praprogramasdeauditorium IdPrAprogramaAuditoriaNavigation { get; set; }
        public virtual ICollection<Praciclocronograma> Praciclocronogramas { get; set; }
        public virtual ICollection<Praciclonormassistema> Praciclonormassistemas { get; set; }
        public virtual ICollection<Pracicloparticipante> Pracicloparticipantes { get; set; }
        public virtual ICollection<Pradireccionespaproducto> Pradireccionespaproductos { get; set; }
        public virtual ICollection<Pradireccionespasistema> Pradireccionespasistemas { get; set; }
    }
}
