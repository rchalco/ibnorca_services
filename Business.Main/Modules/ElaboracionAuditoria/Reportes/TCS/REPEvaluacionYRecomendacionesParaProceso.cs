using Business.Main.DataMapping.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    /// <summary>
    /// PLAN_REG-PRO-TCS-06-01_00 EvaluacionYRecomendacionParaUnProcesoDeCertificacion(Ver 1.0)
    /// </summary>
    public class REPEvaluacionYRecomendacionesParaProceso
    {
        public string Orgaizacion { get; set; }
        public string CodigoDeServicio { get; set; }
        public string FechaDeAuditoria { get; set; }
        public string TipoDeAuditoria { get; set; }
        public string NormasAuditadas { get; set; }
       // public string EquipoAuditoNombreCargo { get; set; }

        public List<RepDesginacionParticipante> EquipoAuditoNombreCargo { get; set; }
        public string ExpertoCertificacion { get; set; }
        public string FechaAsignacionProceso { get; set; }
        public string RedaccionSugerida { get; set; }
    
    }
}
