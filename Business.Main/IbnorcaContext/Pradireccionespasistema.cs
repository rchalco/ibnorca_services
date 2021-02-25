﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Business.Main.IbnorcaContext
{
    public partial class Pradireccionespasistema
    {
        public short IdDireccionPasistema { get; set; }
        public long? IdPrAcicloProgAuditoria { get; set; }
        public int? Correlativo { get; set; }
        public string Oficina { get; set; }
        public string Direccion { get; set; }
        public short? IdparamPais { get; set; }
        public short? IdparamDepartamento { get; set; }
        public string Ciudad { get; set; }
        public int? Dias { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public virtual Praciclosprogauditorium IdPrAcicloProgAuditoriaNavigation { get; set; }
    }
}
