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
        public List<TCPTemario> ListaTemario { get; set; }

    }

    public class TCPTemario
    {
        public string nro { get; set; }
        public string proceso { get; set; }
        public string codigoServicio { get; set; }
        public string idOrganizacion { get; set; }
        public string producto { get; set; }
        public string norma { get; set; }
        public string revision { get; set; }
        public string confirmacion { get; set; }
        public string recomendacion { get; set; }
    }
}
