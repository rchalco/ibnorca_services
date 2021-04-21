using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    /// <summary>
    /// REG-PRO-TCS-05-04.05 Lista de asistencia V.1.0
    /// </summary>
    public class REPListaAsistencia : IObjectReport
    {
        public string NombreEmpresa { get; set; }
        public string CodigoServicio { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string TipoAuditoria { get; set; }
        public string Norma { get; set; }
    }
}
