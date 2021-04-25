using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Pradireccionespaproducto
    {
        public long IdDireccionPaproducto { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Marca { get; set; }
        public string Norma { get; set; }
        public string Sello { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public DateTime? FechaEmisionPrimerCertificado { get; set; }
        public DateTime? FechaVencimientoUltimoCertificado { get; set; }
        public DateTime? FechaVencimientoCertificado { get; set; }
        public string NumeroDeCertificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string EstadoConcer { get; set; }
        public string ReivsionConcer { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
    }
}
