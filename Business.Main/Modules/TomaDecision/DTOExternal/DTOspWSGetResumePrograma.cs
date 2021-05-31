using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.TomaDecision.DTOExternal
{
    public class DTOspWSGetResumePrograma
    {
        public class DetalleTCS
        {
            public int IdCicloNormaSistema { get; set; }
            public int IdPrAcicloProgAuditoria { get; set; }
            public string Norma { get; set; }
            public string Alcance { get; set; }
            public string NumeroDeCertificacion { get; set; }
            public string Direcciones { get; set; }
        }

        public class DetalleTCP
        {
            public long IdDireccionPaproducto { get; set; }
            public long IdPrAcicloProgAuditoria { get; set; }
            public string Nombre { get; set; }
            public string Direccion { get; set; }
            public string Marca { get; set; }
            public string Norma { get; set; }
            public string Sello { get; set; }
            public string NumeroDeCertificacion { get; set; }
        }

        public class Detalle
        {
            public List<DetalleTCS> detalleTCS { get; set; }
            public List<DetalleTCP> detalleTCP { get; set; }
        }
        public string idPrAProgramaAuditoria { get; set; }
        public string CodigoServicioWS { get; set; }
        public string IdServicio { get; set; }
        public string idPrACicloProgAuditoria { get; set; }
        public string Referencia { get; set; }
        public string area { get; set; }
        public string Cliente { get; set; }
        public int IdEtapaDocumento { get; set; }
        public int idCiclo { get; set; }
        public Detalle detalle { get; set; }
    }
}
