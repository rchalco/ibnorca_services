using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Ciclosprogauditorium
    {
        public Ciclosprogauditorium()
        {
            Ciclocronogramas = new HashSet<Ciclocronograma>();
            Ciclonormassistemas = new HashSet<Ciclonormassistema>();
            Cicloparticipantes = new HashSet<Cicloparticipante>();
            Direccionespaproductos = new HashSet<Direccionespaproducto>();
            Direccionespasistemas = new HashSet<Direccionespasistema>();
        }

        public long IdCicloProgAuditoria { get; set; }
        public long? IdProgramaAuditoria { get; set; }
        public string NombreOrganizacionCertificado { get; set; }
        public short? Ano { get; set; }
        public short? IdpTipoAuditoria { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual Programasdeauditorium IdProgramaAuditoriaNavigation { get; set; }
        public virtual ICollection<Ciclocronograma> Ciclocronogramas { get; set; }
        public virtual ICollection<Ciclonormassistema> Ciclonormassistemas { get; set; }
        public virtual ICollection<Cicloparticipante> Cicloparticipantes { get; set; }
        public virtual ICollection<Direccionespaproducto> Direccionespaproductos { get; set; }
        public virtual ICollection<Direccionespasistema> Direccionespasistemas { get; set; }
    }
}
