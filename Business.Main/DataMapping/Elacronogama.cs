using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Elacronogama
    {
        public long IdElAcronograma { get; set; }
        public int? Idelaauditoria { get; set; }
        public long? IdDireccionPaproducto { get; set; }
        public long? IdDireccionPasistema { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Horario { get; set; }
        public string RequisitosEsquema { get; set; }
        public string PersonaEntrevistadaCargo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string ProcesoArea { get; set; }
        public string Auditor { get; set; }
        public string Cargo { get; set; }
        public string Direccion { get; set; }

        public virtual Elaauditorium IdelaauditoriaNavigation { get; set; }
    }
}
