using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListaCertificadosxClienteyTipoDTO
{
    public class RequestListaCertificadosxClienteyTipo : BaseRequest
    {
        public string IdCliente { get; set; }
        public string Tipo { get; set; }
    }
}
