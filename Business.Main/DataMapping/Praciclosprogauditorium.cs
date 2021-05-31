using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Praciclosprogauditorium
    {
        public Praciclosprogauditorium()
        {
            Elaauditoria = new HashSet<Elaauditorium>();
            Praciclocronogramas = new HashSet<Praciclocronograma>();
            Praciclonormassistemas = new HashSet<Praciclonormassistema>();
            Pracicloparticipantes = new HashSet<Pracicloparticipante>();
            Pradireccionespaproductos = new HashSet<Pradireccionespaproducto>();
            Pradireccionespasistemas = new HashSet<Pradireccionespasistema>();
        }

        public long IdPrAcicloProgAuditoria { get; set; }
        public long? IdPrAprogramaAuditoria { get; set; }
        public string NombreOrganizacionCertificado { get; set; }
        public int? IdparamEstadosProgAuditoria { get; set; }
        public string EstadoDescripcion { get; set; }
        public int? Anio { get; set; }
        public int? IdparamTipoAuditoria { get; set; }
        public string Referencia { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? IdParametapaauditoria { get; set; }
        public int? IdEtapaDocumento { get; set; }

        public virtual Parametapaauditorium IdParametapaauditoriaNavigation { get; set; }
        public virtual Praprogramasdeauditorium IdPrAprogramaAuditoriaNavigation { get; set; }
        public virtual ICollection<Elaauditorium> Elaauditoria { get; set; }
        public virtual ICollection<Praciclocronograma> Praciclocronogramas { get; set; }
        public virtual ICollection<Praciclonormassistema> Praciclonormassistemas { get; set; }
        public virtual ICollection<Pracicloparticipante> Pracicloparticipantes { get; set; }
        public virtual ICollection<Pradireccionespaproducto> Pradireccionespaproductos { get; set; }
        public virtual ICollection<Pradireccionespasistema> Pradireccionespasistemas { get; set; }
    }
}
