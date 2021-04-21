using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    public class REPActaMuestreo : IObjectReport
    {

        public string NombreEmpresa { get; set; }
        public string CodigoServicio { get; set; }
        public string FechaInicio { get; set; }
        public string TipoAuditoria { get; set; }
        public string Norma { get; set; }
        public string Fecha { get; set; }


    }
}
