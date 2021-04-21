using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class TCPREPListaAsistenciaCONCER : IObjectReport
    {
        public string Fecha { get; set; }
        public string TipoReunion { get; set; }
        public string Nombre { get; set; }
        public string CargoConcer { get; set; }
        public string Asistencia { get; set; }
    }
}
