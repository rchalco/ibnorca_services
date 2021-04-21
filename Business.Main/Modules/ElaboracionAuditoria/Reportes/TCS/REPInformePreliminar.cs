using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class REPInformePreliminar : IObjectReport
    {

        public string NombreEmpresa { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string TelefonoCelular { get; set; }
        public string CorreoElectronico { get; set; }
        public string CodigoServicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string FechaInicio { get; set; }
        public string Fecha { get; set; }
        public string AuditorLider { get; set; }
        public string Auditor { get; set; }

        public string AuditorEnsayos { get; set; }
        public string Experto { get; set; }
        public string AuditorFormacion { get; set; }


    }
}
