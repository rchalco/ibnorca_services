using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Pracicloparticipante
    {
        public long IdCicloParticipante { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public string IdParticipanteWs { get; set; }
        public string ParticipanteContextWs { get; set; }
        public short? IdpCargoParticipante { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public short? IdpEstadoParticipante { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
    }
}
