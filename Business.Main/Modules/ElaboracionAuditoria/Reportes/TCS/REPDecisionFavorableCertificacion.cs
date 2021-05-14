using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    /// <summary>
    /// PLAN_REG-PRO-TCS-06-02_04_DecisionFavorableDeLaCertificacion (Ver 1.0)
    /// </summary>
     public class REPDecisionFavorableCertificacion : IObjectReport
    {
        public string FechaIbnorca { get; set; }
        public string ReferenciaIbnorca { get; set; }
        public string NombreApellidos { get; set; }
        public string Cargo { get; set; }
        public string Alcance { get; set; }
        public string Sitios { get; set; }
        public string Organizacion { get; set; }
        public string OtorgarRenovarMantener  { get; set; }
        public string NbIso { get; set; }
        public string Etapa { get; set; }
        public string NroCertificacion { get; set; }
        
    }
}
