using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Importacionsolicitud
    {
        public int IdimportacionSolicitud { get; set; }
        public string Nit { get; set; }
        public string Cliente { get; set; }
        public string Detalle { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
