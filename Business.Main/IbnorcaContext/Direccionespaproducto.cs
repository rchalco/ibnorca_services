using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Direccionespaproducto
    {
        public short IdDireccionPaproducto { get; set; }
        public long? IdCicloProgAuditoria { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Marca { get; set; }
        public short? Sello { get; set; }
        public short? IdPais { get; set; }
        public short? IdDepartamento { get; set; }
        public string Ciudad { get; set; }
        public DateTime? FechaEmisionPrimerCertificado { get; set; }
        public DateTime? FechaVencimientoUltimoCertificado { get; set; }
        public DateTime? FechaVencimientoCertificado { get; set; }
        public string NumeroDeCertificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
