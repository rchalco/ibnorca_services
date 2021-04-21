using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    /// <summary>
    /// REG-PRO-TCS-07-01.00 Nota de suspensión de certificacion V.1.0
    /// </summary>
    public class RepNotaSuspensionCertifica : IObjectReport
    {
        public string FechaCabecera { get; set; }
        public string CorrelativoCabecera { get; set; }
        public string NombreNota { get; set; }
        public string Cargo { get; set; }
        public string NombreEmpresa { get; set; }
        public string ReferenciaNota { get; set; }
        public string Texto1 { get; set; }
        public string IbnorcaRTM { get; set; }
        public string NroCertificado { get; set; }
        public string NombreEmpresaTexto { get; set; }
        public string DescripcionOtrogado { get; set; }
        public string Sitios { get; set; }
        public string FechaLiteral1 { get; set; }
        public string Seguimiento { get; set; }
        public string FechaLiteral2 { get; set; }
        public string DirectorEjecutivo { get; set; }

    }
}
