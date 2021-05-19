using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resportes.ReportDTO
{
    class TCPREPResolucionAdministrativa : IObjectReport
    {

        public string Acta { get; set; }
        public string Fecha { get; set; }
        public List<ProductosResolucion> ListaProductosResolucion { get; set; }

    }
    public class ProductosResolucion
    {
        public string proceso { get; set; }
        public string producto { get; set; }
        public string norma { get; set; }
        public string empresa { get; set; }
        public string ubicacion { get; set; }        
    }
}
