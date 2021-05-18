using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    /// <summary>
    /// REG-PRO-TCS-05-07.11 Informe de auditoria V.1.0
    /// </summary>
    public class REPInformeAuditoria : IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string PersonaContacto { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Servicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string Modalidad { get; set; }
        public string Norma { get; set; }
        public string CodigoIAF { get; set; }
        public string FechaAuditoria { get; set; }
        public string DiasAuditor { get; set; }
        public string FechaInforme { get; set; }
        public string TipoFechaProgramada { get; set; }
        public List<RepCronograma> ListCronograma { get; set; }
        public string AlcanceCertificacion { get; set; }
        public List<RepSitiosAlcance> ListAlcanceCert { get; set; }
        public string Confirmacion { get; set; }
        public string ObjetivoAuditoria { get; set; }
        public string NormasEstablecidas { get; set; }
        public List<RepHallazgos> ListResumenHallazgos { get; set; }
        public string RedaccionFortalezas { get; set; }
        public string RedaccionOportunidades { get; set; }
        public string RedaccionNoConformidadesMenor { get; set; }
        public string RedaccionNoConformidadesMayor { get; set; }
        public string CorreoElectronicoAuditor { get; set; }
        public string NombreAuditor { get; set; }
        public string NombreRepresentante { get; set; }
    }

    public class RepCronograma
    {        
        public string NombreCompleto { get; set; }
        public string TotalDiasInSitu { get; set; }
        public string TotalDiasRemoto { get; set; }
    }

    public class RepSitiosAlcance
    {
        public string NombreDireccion { get; set; }
        public string Auditado { get; set; }
    }

    public class RepHallazgos
    {
        public string Fortaleza { get; set; }
        public string OportunidadMejora { get; set; }
        public string NoConformidadMayor { get; set; }
        public string NoConformidadMenor { get; set; }
    }
}
