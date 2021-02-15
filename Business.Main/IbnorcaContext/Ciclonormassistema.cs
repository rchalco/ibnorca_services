using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Ciclonormassistema
    {
        public long IdCicloNormaSistema { get; set; }
        public long? IdCicloProgAuditoria { get; set; }
        public int? IdpNorma { get; set; }
        public string Alcance { get; set; }
        public DateTime? FechaEmisionPrimerCertificado { get; set; }
        public DateTime? FechaVencimientoUltimoCertificado { get; set; }
        public string NumeroDeCertificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual Ciclosprogauditorium IdCicloProgAuditoriaNavigation { get; set; }
    }
}
