using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Praciclonormassistema
    {
        public long IdCicloNormaSistema { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public short? IdparamNorma { get; set; }
        public string Norma { get; set; }
        public string Alcance { get; set; }
        public DateTime? FechaEmisionPrimerCertificado { get; set; }
        public DateTime? FechaVencimientoUltimoCertificado { get; set; }
        public string NumeroDeCertificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
    }
}
