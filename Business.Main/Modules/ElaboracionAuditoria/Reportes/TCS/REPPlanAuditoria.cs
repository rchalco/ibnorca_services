using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Resportes.TCS
{
    /// <summary>
    /// REG-PRO-TCS-05-01.07 Plan de auditoria V.1.0
    /// </summary>
    public class REPPlanAuditoria
    {
        public string NombreEmpresa { get; set; }
        public string CodigoServicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string ModalidadAuditoria { get; set; }
        public string NormaAuditadas { get; set; }
        public string FechaAuditoria { get; set; }
        public string ObjetivosAuditoria { get; set; }
        public string CambiosAlcance { get; set; }
        public string Certificacion { get; set; }
        public string SitiosFisicos { get; set; }
        //public string ListEquipoAuditor { get; set; }
        public string Normas { get; set; }
        public List<RepCronogramaEquipo> ListEquipoAuditor { get; set; }
        public string FechaElaboracion { get; set; }
        public string FechaAprobacion { get; set; }

    }
    public class RepCronogramaEquipo
    {
        public string EquipoAuditor { get; set; }
        public string NombreCompleto { get; set; }
        public string TotalDiasInSitu { get; set; }
        public string TotalDiasRemoto { get; set; }
    }

}
