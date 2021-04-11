using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elacronogama
    {
        public long IdElAcronograma { get; set; }
        public int? Idelaauditoria { get; set; }
        public long? IdCicloParticipante { get; set; }
        public long? IdDireccionPaproducto { get; set; }
        public long? IdDireccionPasistema { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Horario { get; set; }
        public string RequisitosEsquema { get; set; }
        public string PersonaEntrevistadaCargo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Pracicloparticipante IdCicloParticipanteNavigation { get; set; }
        public virtual Pradireccionespaproducto IdDireccionPaproductoNavigation { get; set; }
        public virtual Pradireccionespasistema IdDireccionPasistemaNavigation { get; set; }
        public virtual Elaauditorium IdelaauditoriaNavigation { get; set; }
    }
}
