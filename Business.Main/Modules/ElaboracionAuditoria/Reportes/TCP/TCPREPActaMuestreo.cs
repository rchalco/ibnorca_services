using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP
{
    public class TCPREPActaMuestreo : IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string CodigoServicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string Norma { get; set; }
        public string Fecha { get; set; }
        public string PlanMuestreo { get; set; }
        public string Interno { get; set; }
        public string Externo { get; set; }
        public string DescripcionExterno { get; set; }

    }
}
