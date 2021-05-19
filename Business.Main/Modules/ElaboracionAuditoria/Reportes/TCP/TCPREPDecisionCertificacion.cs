using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    public class TCPREPDecisionCertificacion : IObjectReport
    {
        public class ProductoDecision
        {
            public int Nro { get; set; }
            public string Producto { get; set; }
            public string Norma { get; set; }
        }

        public class Calendario
        {
            public string Proceso { get; set; }
            public string Fecha { get; set; }
        }

        public string Fecha { get; set; }
        public string TipoAuditoria { get; set; }
        public string NumeroCertificado { get; set; }
        public string NombreEmpresa { get; set; }
        public List<ProductoDecision> ListProductos { get; set; }
        public List<Calendario> ListCalendario { get; set; }
    }
   
}
