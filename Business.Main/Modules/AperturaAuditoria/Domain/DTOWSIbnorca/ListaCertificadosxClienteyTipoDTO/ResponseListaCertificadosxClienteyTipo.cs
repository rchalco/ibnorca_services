using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListaCertificadosxClienteyTipoDTO
{
    public class ListaCertifcado
    {
        public string IdCertificadoServicios { get; set; }
        public string IdServicio { get; set; }
        public string idCliente { get; set; }
        public string Codigo { get; set; }
        public string tipoCertificado { get; set; }
        public string cliente { get; set; }
        public string FechaEmision { get; set; }
        public string FechaValido { get; set; }
        public string estado { get; set; }
        public string idEstado { get; set; }
        public string ProductoServicio { get; set; }
        public string Alcance { get; set; }
    }

    public class ResponseListaCertificadosxClienteyTipo
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int IdCliente { get; set; }
        public string Tipo { get; set; }
        public int TotalCertificados { get; set; }
        public List<ListaCertifcado> ListaCertifcados { get; set; }
    }


}
