using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    /// <summary>
    /// REG-PRO-TCS-05-05.00 Lista de verificación auditor V.1.0
    /// </summary>
    public class REPListaVerificacionAuditor : IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string Sitios { get; set; }
        public string TipoAuditoria { get; set; }
        public string Norma { get; set; }
        public string FechaAuditoria { get; set; }
        public string Usuario { get; set; }
        public string Cargo { get; set; }
        public string CodigoServicio { get; set; }
        public string ProcesoAuditado { get; set; }
        public string NombreAuditado { get; set; }
        public string Fecha { get; set; }
        public string SitiosAuditado { get; set; }
    }
}
