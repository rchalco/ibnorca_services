using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class TCPREPPlanAccion : IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string TipoAuditoria { get; set; }
        public string Norma { get; set; }
        public string Fecha { get; set; }
        public string AuditorLider { get; set; }
    }
}
