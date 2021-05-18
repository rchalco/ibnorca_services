using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Praciclocronograma
    {
        public long IdCiclosCronogramas { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public decimal? DiasInsitu { get; set; }
        public decimal? DiasRemoto { get; set; }
        public string HorarioTrabajo { get; set; }
        public DateTime? MesProgramado { get; set; }
        public DateTime? MesReprogramado { get; set; }
        public DateTime? FechaInicioDeEjecucionDeAuditoria { get; set; }
        public DateTime? FechaDeFinDeEjecucionAuditoria { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public decimal? DiasPresupuesto { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
    }
}
