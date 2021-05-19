using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class TCPREPNotaSuspencionCertificado : IObjectReport
    {

        public string Fecha { get; set; }
        public string MarcaComercial { get; set; }
        public string Sitio { get; set; }
        public string NumeroCertificado { get; set; }
        public string NombreEmpresa { get; set; }
        public string Producto { get; set; }

    }
}
