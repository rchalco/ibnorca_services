using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP
{
    /// <summary>
    /// REG-PRO-TCP-05-01.01 Plan de Auditoria
    /// </summary>
    public class TCPREPPlanAuditoria : IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string TelefonoCelular { get; set; }
        public string CorreoElectronico { get; set; }
        public string CodigoServicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string FechaInicio { get; set; }
        public string Productos { get; set; }
        public string Ensayos { get; set; }
        public string Criterios { get; set; }
        public string FechaElaboracionPlan { get; set; }
        public string CambiosAlcances { get; set; }
        
        public List<RepEquipoTCP> ListEquipoAuditor { get; set; }
        public List<RepCronogramaEquipoTCP> ListCronograma { get; set; }
        

    }

    public class RepEquipoTCP
    {
        public string EquipoAuditor { get; set; }
        public string NombreCompleto { get; set; }
        public string TotalDiasInSitu { get; set; }
        public string TotalDiasRemoto { get; set; }
    }

    public class RepCronogramaEquipoTCP
    {
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string SitioAuditado { get; set; }
        public string RequisitoEsquema { get; set; }
        public string EquipoAuditado { get; set; }
        public string ResponsableOrganiza { get; set; }
    }
}
