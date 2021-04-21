using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP
{
    public class TCPREPListaVerificacionAuditor : IObjectReport
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
        public string Hallazgos { get; set; }

    }
}
