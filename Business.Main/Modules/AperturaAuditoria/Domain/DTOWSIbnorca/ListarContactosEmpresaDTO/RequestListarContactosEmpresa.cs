using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.ListarContactosEmpresaDTO
{
    public class RequestListarContactosEmpresa: BaseRequest
    {
        public string IdCliente { get; set; }
    }
}
