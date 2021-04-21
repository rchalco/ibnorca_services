using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    public class TCPREPActaReunion : IObjectReport
    {

        public string Acta { get; set; }
        public string Fecha { get; set; }
        public string Modalidad { get; set; }
        public string Hora { get; set; }
        
    }
}
