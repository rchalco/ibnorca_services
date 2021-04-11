using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP
{
    public class TCPREPListaVerificacionReunionApertura
    {
        public string NombreEmpresa{ get; set; }
        public string CodigoServicio { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string TipoAuditoria { get; set; }
        public string AuditorLider { get; set; }
    }
}
