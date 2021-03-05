using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Placronogama
    {
        public long IdPlAcronograma { get; set; }
        public long? IdPlaPlanEtapa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string SitioCronograma { get; set; }
        public string PersonaEntrevistadaCargo { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
