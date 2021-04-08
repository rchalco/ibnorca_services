using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Pracicloparticipante
    {
        public long IdCicloParticipante { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public int? IdParticipanteWs { get; set; }
        public string ParticipanteDetalleWs { get; set; }
        public int? IdCargoWs { get; set; }
        public string CargoDetalleWs { get; set; }
        public decimal? DiasInsistu { get; set; }
        public decimal? DiasRemoto { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
    }
}
