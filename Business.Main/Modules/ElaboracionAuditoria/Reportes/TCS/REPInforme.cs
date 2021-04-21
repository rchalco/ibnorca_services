using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    public class REPInforme : IObjectReport
    {
        public string IDCliente { get; set; }
        public string TipoAuditoria { get; set; }
        public string Productos { get; set; }
        public string Norma { get; set; }
        public string FechaInicio { get; set; }
    }
}
