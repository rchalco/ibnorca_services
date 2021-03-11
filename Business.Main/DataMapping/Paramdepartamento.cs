using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.DataMapping
{
    public partial class Paramdepartamento
    {
        public int IdparamDepartamento { get; set; }
        public int? IdparamPais { get; set; }
        public string Departamento { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
