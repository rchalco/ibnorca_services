using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Direccionespasistema1
    {
        public short IdDireccionPasistema { get; set; }
        public long? IdCicloProgAuditoria { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public short? IdPais { get; set; }
        public short? IdDepartamento { get; set; }
        public string Ciudad { get; set; }
        public int? NumeroDeDias { get; set; }
        public long? IdUsuarioUltimoCambio { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
    }
}
