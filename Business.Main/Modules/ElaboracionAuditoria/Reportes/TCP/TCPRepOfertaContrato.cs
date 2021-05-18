using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCP
{
    public class TCPRepOfertaContrato : IObjectReport
    {
        public string Referencia { get; set; }
        public string FechaIbnorca { get; set; }
        public string Cliente { get; set; }
        public string DireccionCliente { get; set; }
        public string Guia { get; set; }
        public string NombreGerente { get; set; }
        public string mailIbnorca { get; set; }
        public List<ProductoOferta> ListProductos { get; set; }
        public List<PresupuestoOfertaTCP> ListPresupuesto { get; set; }
        public class ProductoOferta
        {
            public string Nombre { get; set; }
            public string Marca { get; set; }
            public string Norma { get; set; }
            public string NroSello { get; set; }
            public string Direccion { get; set; }
        }
        public class PresupuestoOfertaTCP
        {
            public string Etapa { get; set; }
            public string Concepto { get; set; }
            public string DiasAuditor { get; set; }
            public string CostoUSD { get; set; }
        }


    }
}
