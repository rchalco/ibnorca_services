using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Paramnorma
    {
        public short IdparamNorma { get; set; }
        public string CodigoDeNorma { get; set; }
        public string Norma { get; set; }
        public short? IdpArea { get; set; }
        public string PathNorma { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
