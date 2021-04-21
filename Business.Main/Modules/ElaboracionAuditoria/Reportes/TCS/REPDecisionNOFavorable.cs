using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    /// <summary>
    /// PLAN_REG-PRO-TCS-06-03_00_DecisionNoFavorable (Ver 1.0)
    /// </summary>
    public class REPDecisionNOFavorable : IObjectReport
    {
        public string FechaIbnorca { get; set; }
        public string ReferenciaIbnorca { get; set; }
        public string NombreApellidos { get; set; }
        public string Cargo { get; set; }
        public string Organizacion { get; set; }
        public string OtorgarRenovar { get; set; }
        public string NbIso { get; set; }
        public string CertificacionRenovacion { get; set; }
        public string SistemaDeGestionNB { get; set; }
        public string ConsideracionesNumeradas { get; set; }
        public string Seguimiento { get; set; }

        public string NroCertificadoIbnorca { get; set; }
        public string Alcance { get; set; }
        public string Sitios { get; set; }

    }
}
