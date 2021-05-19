using Business.Main.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.ElaboracionAuditoria.Reportes.TCS
{
    public class TCSRepOfertaContrato : IObjectReport
    {
        public string Referencia { get; set; }
        public string FechaIbnorca { get; set; }
        public string Cliente { get; set; }
        public string NombreGerente { get; set; }
        public string Norma { get; set; }
        public string Alcance { get; set; }
        public string Guia { get; set; }
        public List<SitosOferta> ListSitios { get; set; }
        public List<PresupuestoOfertaTCS> ListPresupuesto { get; set; }

        public class SitosOferta
        {
            public string Sitio { get; set; }
        }

        public class PresupuestoOfertaTCS
        {
            public string Etapa { get; set; }
            public string Concepto { get; set; }
            public string DiasAuditor { get; set; }
            public string CostoUSD { get; set; }
        }
    }
}
