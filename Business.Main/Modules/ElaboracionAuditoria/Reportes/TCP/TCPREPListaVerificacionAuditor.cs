using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class TCPREPListaVerificacionAuditor
    {
        public string NombreEmpresa { get; set; }
        public string Sitios { get; set; }
        public string TipoAuditoria { get; set; }
        public string Norma { get; set; }
        public string Fecha { get; set; }
        public string Usuario { get; set; }
        public string Cargo { get; set; }




        public string ProcesoAuditado { get; set; }
        public string NombreAuditado { get; set; }
        public string Sitio { get; set; }
    }
}
