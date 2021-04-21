using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{

    /// <summary>
    /// PLAN_REG-PRO-TCS-05-08.03 Datos de la organizacion (Ver 1.0)    /// </summary>
    public class REPDatosDeLaOrganizacion : IObjectReport
    {
        public string FechaDeAuditoria { get; set; }
        public string TipoDeAuditoria { get; set; }
        public string NombreDeLaOrganizacion { get; set; }
        public string Norma { get; set; }
        public string Alcance { get; set; }
        public string Sitios { get; set; }
        public string NombreAuditorLider { get; set; }
        public string NombreCargoRepresentanteOrg { get; set; }
        public string CoordinadorAuditoria { get; set; }
        public string FechaActual { get; set; }
    }
}
