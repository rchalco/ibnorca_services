using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Pdepartamento
    {
        public int IdpDepartamento { get; set; }
        public int? IdpPais { get; set; }
        public string Departamento { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
