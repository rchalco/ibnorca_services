using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Pnorma
    {
        public short IdpNorma { get; set; }
        public string CodigoDeNorma { get; set; }
        public string Norma { get; set; }
        public short? IdpArea { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
