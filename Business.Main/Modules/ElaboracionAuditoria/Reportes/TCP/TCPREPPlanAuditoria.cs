using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    /// <summary>
    /// REG-PRO-TCP-05-01.01 Plan de Auditoria
    /// </summary>
    public class TCPREPPlanAuditoria
    {
        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string TelefonoCelular { get; set; }
        public string CorreoElectronico { get; set; }
        public string CodigoServicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string FechaInicio { get; set; }

    }
}
