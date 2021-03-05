using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Plaplanetapa
    {
        public long IdPlaPlanEtapa { get; set; }
        public long? IdPlAauditoria { get; set; }
        public string FechaInicioPlan { get; set; }
        public DateTime? FechaDeElaboracionDePa { get; set; }
        public DateTime? FechaDeAprobacionDeCliente { get; set; }
        public string QuejaVc { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
