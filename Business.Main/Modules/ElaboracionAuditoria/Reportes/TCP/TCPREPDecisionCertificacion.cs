using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class TCPREPDecisionCertificacion : IObjectReport
    {

        public string Fecha { get; set; }
        public string TipoAuditoria { get; set; }
        public string NumeroCertificado { get; set; }
        public string NombreEmpresa { get; set; }
       
    }
}
