using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Modules.AperturaAuditoria.Domain.DTOWSIbnorca.BuscarxIdClienteEmpresaDTO
{
    public class RequestBusquedaCliente : BaseRequest
    {
        public string IdCliente { get; set; }
    }
}
