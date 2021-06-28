using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.TomaDecision.DTOExternal
{
    public class DTOspWSGetDetalleProgramaTCP
    {
        public string idPrAProgramaAuditoria { get; set; }
        public string CodigoServicioWS { get; set; }
        public string IdServicio { get; set; }
        public string idPrACicloProgAuditoria { get; set; }
        public string Referencia { get; set; }
        public string area { get; set; }
        public string Cliente { get; set; }
        public int IdEtapaDocumento { get; set; }
        public int idCiclo { get; set; }
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string direccion { get; set; }
        public string norma { get; set; }
        public string numeroDeCertificacion { get; set; }
    }
}
