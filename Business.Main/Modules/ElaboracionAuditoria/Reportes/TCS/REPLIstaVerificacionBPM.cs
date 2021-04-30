using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    /// <summary>
    /// PLAN_REG-PRO-TCS-05-05B_00 Lista de verificación auditoria BPM - (Ver 1.0)
    /// </summary>
    public class REPLIstaVerificacionBPM : IObjectReport
    {
        public string Organizacion { get; set; }
        public string SitiosAuditados { get; set; }

        public string TipoAuditoria { get; set; }

        public string FechasDeAuditoria { get; set; }

        public string NombreYApellidos { get; set; }
        public string ResponsabilidadORol { get; set; }

        public string ProcesoAuditado { get; set; }
        public string NombreAuditado { get; set; }
        public string Fecha { get; set; }
        public string SitiosAuditado { get; set; }
    }
}
