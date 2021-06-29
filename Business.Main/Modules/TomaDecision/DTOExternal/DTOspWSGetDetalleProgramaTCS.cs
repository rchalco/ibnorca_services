using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.TomaDecision.DTOExternal
{
    public class DTOspWSGetDetalleProgramaTCS
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
        public int idSistema { get; set; }
        public string norma { get; set; }
        public string alcance { get; set; }
        public string numeroDeCertificacion { get; set; }
    }
}
