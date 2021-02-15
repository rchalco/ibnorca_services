using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Ciclocronograma
    {
        public long IdCiclosCronogramas { get; set; }
        public long? IdCicloProgAuditoria { get; set; }
        public int? CantidadDeDiasTotal { get; set; }
        public short? MesProgramado { get; set; }
        public short? MesReprogramado { get; set; }
        public DateTime? FechaInicioDeEjecucionDeAuditoria { get; set; }
        public DateTime? FechaDeFinDeEjecucionAuditoria { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
