using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Direccionespasistema
    {
        public short IdDireccionPasistema { get; set; }
        public long? IdCicloProgAuditoria { get; set; }
        public int? Correlativo { get; set; }
        public string Oficina { get; set; }
        public string Direccion { get; set; }
        public short? IdPais { get; set; }
        public short? IdDepartamento { get; set; }
        public string Ciudad { get; set; }
        public int? Dias { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
